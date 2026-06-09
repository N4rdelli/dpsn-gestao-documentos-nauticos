using dpsn_gestao_documentos_nauticos.Data;
using dpsn_gestao_documentos_nauticos.Models;
using dpsn_gestao_documentos_nauticos.Services;
using dpsn_gestao_documentos_nauticos.Settings;
using dpsn_gestao_documentos_nauticos.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;
using MongoDB.Driver;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace dpsn_gestao_documentos_nauticos.Controllers
{
    [Authorize] // Garante que apenas usuários logados acessem
    public class DocumentoController : Controller
    {
        private readonly MongoDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailService _emailService;
        private readonly EmailSettings _emailSettings;

        public DocumentoController(MongoDbContext context, UserManager<ApplicationUser> userManager, EmailService emailService, IOptions<EmailSettings> emailSettings)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _emailSettings = emailSettings.Value;

            QuestPDF.Settings.License = LicenseType.Community;
        }

        // Listagem de Documentos
        public async Task<IActionResult> Index()
        {
            List<Documento> documentos;

            if (User.IsInRole("Estaleiro"))
            {
                // O estaleiro vê TODOS os seus documentos, 
                // para poder editar e visualizar detalhes. A regra do PDF assinado fica na View.
                var userId = _userManager.GetUserId(User);
                documentos = await _context.Documentos
                    .Find(d => d.Estaleiro.Id == userId)
                    .ToListAsync();
            }
            else
            {
                // Tecnologo e Admin visualizam tudo
                documentos = await _context.Documentos.Find(_ => true).ToListAsync();
            }

            return View(documentos);
        }

        // GET: Documento/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new DocumentoViewModel();

            if (User.IsInRole("Tecnologo") || User.IsInRole("Admin"))
            {
                // Carrega todos os estaleiros para o dropdown do Tecnólogo/Admin
                viewModel.Estaleiros = await _context.Estaleiros.Find(_ => true).ToListAsync();
                viewModel.Embarcacoes = new List<Embarcacao>(); // Começa vazio, JavaScript preenche
            }
            else if (User.IsInRole("Estaleiro"))
            {
                // Se for Estaleiro, fixa o ID dele e traz apenas as suas embarcações
                var userId = _userManager.GetUserId(User);
                viewModel.EstaleiroId = userId;
                viewModel.Embarcacoes = await _context.Embarcacoes.Find(e => e.EstaleiroId == userId).ToListAsync();
            }

            return View(viewModel);
        }

        // POST: Documento/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DocumentoViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Busca os objetos completos do Estaleiro e da Embarcação do banco
                    var estaleiro = await _context.Estaleiros.Find(e => e.Id == model.EstaleiroId).FirstOrDefaultAsync();
                    var embarcacao = await _context.Embarcacoes.Find(e => e.IdEmbarcacao == model.EmbarcacaoId).FirstOrDefaultAsync();

                    if (estaleiro == null || embarcacao == null)
                    {
                        ModelState.AddModelError(string.Empty, "Estaleiro ou Embarcação inválidos.");
                        return await PreencherListasErro(model);
                    }

                    // 2. Monta o objeto Cliente completo com o Endereço
                    var cliente = new Cliente
                    {
                        Ano_contrucao = model.Ano_contrucao,
                        NumeroChassi = model.NumeroChassi,
                        Nome = model.Nome,
                        Cpf_cnpj = model.Cpf_cnpj,
                        Endereco = new Endereco
                        {
                            Cep = model.Cep,
                            Logradouro = model.Logradouro,
                            Numero = model.Numero,
                            Complemento = model.Complemento,
                            Bairro = model.Bairro,
                            Cidade = model.Cidade,
                            Estado = model.Estado
                        }
                    };

                    // 3. Instancia o Documento final guardando os objetos mapeados (comportamento de documento/histórico)
                    var documento = new Documento
                    {
                        Estaleiro = estaleiro,
                        Embarcacao = embarcacao,
                        Cliente = cliente,
                        NumeroInscricao = model.NumeroInscricao,
                        DataCriacaoDocumento = DateTime.UtcNow,
                        StatusAssinatura = false
                    };

                    // Salva no MongoDB
                    await _context.Documentos.InsertOneAsync(documento);

                    TempData["MensagemSucesso"] = "Documento gerado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = "Erro inesperado ao gerar o documento.";
                    Console.WriteLine(ex.Message);
                }
            }

            return await PreencherListasErro(model);
        }
        // GET: Documento/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var documento = await _context.Documentos.Find(d => d.Id == id).FirstOrDefaultAsync();
            if (documento == null) return NotFound();

            // Mapeia do Banco para a ViewModel
            var viewModel = new DocumentoViewModel
            {
                EstaleiroId = documento.Estaleiro?.Id,
                EmbarcacaoId = documento.Embarcacao?.IdEmbarcacao,
                Ano_contrucao = documento.Cliente?.Ano_contrucao,
                NumeroChassi = documento.Cliente?.NumeroChassi,
                Nome = documento.Cliente?.Nome,
                Cpf_cnpj = documento.Cliente?.Cpf_cnpj,
                Cep = documento.Cliente?.Endereco?.Cep,
                Logradouro = documento.Cliente?.Endereco?.Logradouro,
                Numero = documento.Cliente?.Endereco?.Numero,
                Complemento = documento.Cliente?.Endereco?.Complemento,
                Bairro = documento.Cliente?.Endereco?.Bairro,
                Cidade = documento.Cliente?.Endereco?.Cidade,
                Estado = documento.Cliente?.Endereco?.Estado,
                NumeroInscricao = documento.NumeroInscricao
            };

            // Aproveita o método privado para preencher os Dropdowns
            return await PreencherListasErro(viewModel);
        }

        // POST: Documento/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, DocumentoViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var documentoNoBanco = await _context.Documentos.Find(d => d.Id == id).FirstOrDefaultAsync();
                    if (documentoNoBanco == null) return NotFound();

                    var estaleiro = await _context.Estaleiros.Find(e => e.Id == model.EstaleiroId).FirstOrDefaultAsync();
                    var embarcacao = await _context.Embarcacoes.Find(e => e.IdEmbarcacao == model.EmbarcacaoId).FirstOrDefaultAsync();

                    if (estaleiro == null || embarcacao == null)
                    {
                        ModelState.AddModelError(string.Empty, "Estaleiro ou Embarcação inválidos.");
                        return await PreencherListasErro(model);
                    }

                    // Atualiza os dados do documento
                    documentoNoBanco.Estaleiro = estaleiro;
                    documentoNoBanco.Embarcacao = embarcacao;
                    documentoNoBanco.NumeroInscricao = model.NumeroInscricao;

                    documentoNoBanco.Cliente = new Cliente
                    {
                        Ano_contrucao = model.Ano_contrucao,
                        NumeroChassi = model.NumeroChassi,
                        Nome = model.Nome,
                        Cpf_cnpj = model.Cpf_cnpj,
                        Endereco = new Endereco
                        {
                            Cep = model.Cep,
                            Logradouro = model.Logradouro,
                            Numero = model.Numero,
                            Complemento = model.Complemento,
                            Bairro = model.Bairro,
                            Cidade = model.Cidade,
                            Estado = model.Estado
                        }
                    };

                    await _context.Documentos.ReplaceOneAsync(d => d.Id == id, documentoNoBanco);
                    TempData["MensagemSucesso"] = "Documento atualizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = "Erro inesperado ao editar o documento.";
                    Console.WriteLine(ex.Message);
                }
            }

            return await PreencherListasErro(model);
        }
        // GET: Documento/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var documento = await _context.Documentos.Find(d => d.Id == id).FirstOrDefaultAsync();
            if (documento == null) return NotFound();

            return View(documento);
        }

        // POST: Documento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            await _context.Documentos.DeleteOneAsync(d => d.Id == id);
            TempData["MensagemSucesso"] = "Documento deletado com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        // GET: Documento/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null) return NotFound();

            var documento = await _context.Documentos.Find(d => d.Id == id).FirstOrDefaultAsync();
            if (documento == null) return NotFound();

            return View(documento);
        }

        #region Endpoints Auxiliares (Chamadas via JavaScript)

        // Endpoint REST para carregar as embarcações de um estaleiro selecionado na View
        [HttpGet]
        public async Task<IActionResult> GetEmbarcacoesPorEstaleiro(string estaleiroId)
        {
            if (string.IsNullOrEmpty(estaleiroId)) return Json(new List<Embarcacao>());

            var embarcacoes = await _context.Embarcacoes
                .Find(e => e.EstaleiroId == estaleiroId)
                .ToListAsync();

            return Json(embarcacoes);
        }

        #endregion

        #region Métodos Privados
        private async Task<IActionResult> PreencherListasErro(DocumentoViewModel model)
        {
            if (User.IsInRole("Tecnologo") || User.IsInRole("Admin"))
            {
                model.Estaleiros = await _context.Estaleiros.Find(_ => true).ToListAsync();
                if (!string.IsNullOrEmpty(model.EstaleiroId))
                {
                    model.Embarcacoes = await _context.Embarcacoes.Find(e => e.EstaleiroId == model.EstaleiroId).ToListAsync();
                }
            }
            else
            {
                var userId = _userManager.GetUserId(User);
                model.Embarcacoes = await _context.Embarcacoes.Find(e => e.EstaleiroId == userId).ToListAsync();
            }
            return View(model);
        }
        #endregion

        public async Task<IActionResult> DownloadMinuta(string id)
        {
            var doc = await _context.Documentos.Find(d => d.Id == id).FirstOrDefaultAsync();
            if (doc == null) return NotFound();

            // Gera o PDF base estruturado pelo QuestPDF
            byte[] pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(11));
                    page.Header().Text($"TERMO DE RESPONSABILIDADE DE CONSTRUÇÃO").FontSize(14).Bold().Underline();
                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Certifico, para comprovação perante a Capitania dos portos, que a embarcação modelo {doc.Embarcacao.Nome}, foi construída por {doc.Estaleiro?.RazaoSocial}, CNPJ {doc.Estaleiro?.Cnpj}, com as seguintes características:");
                        col.Spacing(10);
                        col.Item().Text($"a-) Comprimento Total: {doc.Embarcacao?.ComprimentoTotal}");
                        col.Item().Text($"b-) Boca Moldada: {doc.Embarcacao?.BocaMoldada}");
                        col.Item().Text($"c-) Pontal Moldado: {doc.Embarcacao?.PontalMoldado}");
                        col.Item().Text($"d-) Calado Máximo: {doc.Embarcacao?.CaladoMaximo}");
                        col.Item().Text($"e-) Calado Leve: {doc.Embarcacao?.CaladoLeve}");
                        col.Item().Text($"f-) Arqueação Bruta:  {doc.Embarcacao?.ArqueacaoBruta}");
                        col.Item().Text($"g-) Arqueação Liquida:  {doc.Embarcacao?.ArqueacaoLiquida}");
                        col.Item().Text($"h-) TPB: {doc.Embarcacao?.Tpb}");
                        col.Item().Text($"i-) Contorno: {doc.Embarcacao?.Contorno}");
                        col.Item().Text($"j-) Lastro: {doc.Embarcacao?.Lastro ?? 0}");
                        col.Item().Text($"k-) Área de Navegação / Tipo de serviço: {doc.Embarcacao?.AreaNavegacaoTipoServico}");
                        col.Item().Text($"l-) Tipo de Embarcação: {doc.Embarcacao?.TipoEmbarcacao}");
                        col.Item().Text($"m-) Material do Casco: {doc.Embarcacao?.MaterialCasco}");
                        col.Item().Text($"n-) Motorização Máxima: {doc.Embarcacao?.MotorizacaoMax}");
                        col.Item().Text($"o-) Motorização Mínima: {doc.Embarcacao?.MotorizacaoMin}");
                        col.Item().Text($"p-) Construtor: {doc.Estaleiro?.RazaoSocial}");
                        col.Item().Text($"q-) Ano de Construção: {doc.Cliente?.Ano_contrucao}");
                        col.Item().Text($"r-) N° Casco/Chassi: {doc.Cliente?.NumeroChassi}");
                        col.Item().Text($"s-) Modelo: {doc.Embarcacao?.Nome}");
                        col.Item().Text($"t-) N° de Inscrição: {doc.NumeroInscricao}");
                        col.Item().Text($"u-) Armador: {doc.Cliente?.Nome} / CNPJ {doc.Cliente?.Cpf_cnpj}");
                        col.Item().Text($"v-) Endereço: {doc.Cliente?.Endereco.Logradouro}, n° {doc.Cliente?.Endereco.Numero} {doc.Cliente?.Endereco.Complemento} - " +
                            $"{doc.Cliente?.Endereco.Bairro} - {doc.Cliente?.Endereco.Cidade}/{doc.Cliente?.Endereco.Estado} - CEP {doc.Cliente?.Endereco.Cep}");
                        col.Spacing(5);
                        col.Item().Text($"Atende as prescrições aplicáveis constantes na NORMAM-211/DPC e apresenta condições de segurança, estabilidade e estruturais satisfatórias," +
                            $"para operar com a seguinte capacidade de pessoas:");
                        col.Item().Text($"Tripulantes: {doc.Embarcacao?.Tripulantes}");
                        col.Item().Text($"Passageiros: {doc.Embarcacao?.Passageiros}");
                        col.Item().Text($"Certifico, ainda que a embarcação foi construída em comformidade com as normas e regulamentos nacionas em vigor.");
                        col.Item().Text($"Declaro outrossim que qualquer modificação de lastreamento, tancagem, arranjo geral ou alterações de qualquer monta," +
                            $"bem como incidentes ou sinistros, invalidam a presente declaração");
                        col.Item().Text($"{doc.Estaleiro?.Endereco.Estado}, {DateTime.UtcNow:d 'de' MMMM 'de' yyyy}");
                        col.Item().Text($"___________________________");
                        col.Item().Text($"Tecnólogo Naval Responsável");
                        col.Item().Text($"Helcio Marcelo De Russi");
                        col.Item().Text($"CREA: 5060478012");
                    });
                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span($"{doc.Estaleiro?.Endereco.Logradouro}, n° {doc.Estaleiro?.Endereco.Numero} - Bairro: {doc.Estaleiro?.Endereco.Bairro} " +
                            $"- {doc.Estaleiro?.Endereco.Cidade} " +
                            $"- {doc.Estaleiro?.Endereco.Estado} -" +
                            $" CEP: {doc.Estaleiro?.Endereco.Cep} - CNPJ: {doc.Estaleiro?.Cnpj} ");
                    });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Minuta_{doc.Id}.pdf");
        }

        // 3. REENVIO DO PDF ASSINADO (Upload do Tecnólogo)
        [HttpPost]
        [Authorize(Roles = "Tecnologo,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssinarEReenviar(string id, IFormFile arquivoPdf)
        {
            if (arquivoPdf == null || arquivoPdf.Length == 0)
            {
                TempData["MensagemErro"] = "Selecione um arquivo PDF válido.";
                return RedirectToAction(nameof(Index));
            }

            var pastaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documentos_assinados");
            if (!Directory.Exists(pastaDestino)) Directory.CreateDirectory(pastaDestino);

            var nomeArquivo = $"Doc_Assinado_{id}.pdf";
            var caminhoCompleto = Path.Combine(pastaDestino, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await arquivoPdf.CopyToAsync(stream);
            }

            // Atualiza o Status para True e vincula o arquivo físico
            var filter = Builders<Documento>.Filter.Eq(d => d.Id, id);
            var update = Builders<Documento>.Update
                .Set(d => d.StatusAssinatura, true)
                .Set(d => d.CaminhoPdfAssinado, nomeArquivo)
                .Set(d => d.DataAssinatura, DateTime.UtcNow);

            await _context.Documentos.UpdateOneAsync(filter, update);

            TempData["MensagemSucesso"] = "Documento assinado e reenviado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // 4. DOWNLOAD DO PDF JÁ ASSINADO (Usado pelo Estaleiro e Tecnólogo)
        public async Task<IActionResult> DownloadPdfAssinado(string id)
        {
            var doc = await _context.Documentos.Find(d => d.Id == id).FirstOrDefaultAsync();
            if (doc == null || string.IsNullOrEmpty(doc.CaminhoPdfAssinado)) return NotFound();

            var caminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documentos_assinados", doc.CaminhoPdfAssinado);
            if (!System.IO.File.Exists(caminhoArquivo)) return NotFound();

            byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(caminhoArquivo);
            return File(fileBytes, "application/pdf", doc.CaminhoPdfAssinado);
        }

        // 5. TELA DE ENVIO EM LOTE (GET)
        [Authorize(Roles = "Tecnologo,Admin")]
        public async Task<IActionResult> EnviarLote()
        {
            // Busca apenas os documentos que já foram assinados e possuem arquivo
            var assinados = await _context.Documentos
                .Find(d => d.StatusAssinatura == true && d.CaminhoPdfAssinado != null)
                .ToListAsync();

            return View(assinados);
        }

        // 6. PROCESSAR ENVIO EM LOTE POR E-MAIL (POST)
        [HttpPost]
        [Authorize(Roles = "Tecnologo,Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessarEnviarLote(List<string> documentosSelecionados)
        {
            if (documentosSelecionados == null || documentosSelecionados.Count == 0)
            {
                TempData["MensagemErro"] = "Nenhum documento foi selecionado para envio.";
                return RedirectToAction(nameof(EnviarLote));
            }

            try
            {
                // Busca no banco todos os documentos passados na lista de IDs selecionados
                var docs = await _context.Documentos
                    .Find(Builders<Documento>.Filter.In(d => d.Id, documentosSelecionados))
                    .ToListAsync();

                // Agrupa os documentos pelo e-mail do Estaleiro para não mandar vários e-mails repetidos
                var agrupadoPorEstaleiro = docs.GroupBy(d => d.Estaleiro.Email);

                foreach (var grupo in agrupadoPorEstaleiro)
                {
                    var emailEstaleiro = grupo.Key;
                    var nomeEstaleiro = grupo.First().Estaleiro.NomeFantasia;

                    // Instancia a estrutura do MailKit/MimeKit usando os seus EmailSettings originais
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                    email.To.Add(MailboxAddress.Parse(emailEstaleiro));
                    email.Subject = "Documentos Nauticos Assinados Disponíveis";

                    var builder = new BodyBuilder
                    {
                        HtmlBody = $"<h3>Olá, {nomeEstaleiro}</h3><p>O Tecnólogo realizou o envio dos documentos assinados das suas embarcações em anexo.</p>"
                    };

                    // Anexa cada arquivo físico selecionado pertencente a este estaleiro
                    foreach (var doc in grupo)
                    {
                        var caminhoArquivo = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "documentos_assinados", doc.CaminhoPdfAssinado);
                        if (System.IO.File.Exists(caminhoArquivo))
                        {
                            byte[] pdfBytes = await System.IO.File.ReadAllBytesAsync(caminhoArquivo);
                            builder.Attachments.Add(doc.CaminhoPdfAssinado, pdfBytes);
                        }
                    }

                    email.Body = builder.ToMessageBody();

                    // Conexão SMTP limpa baseada no seu AccountsController/EmailService
                    using var smtp = new MailKit.Net.Smtp.SmtpClient();
                    await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }

                TempData["MensagemSucesso"] = $"{docs.Count} documento(s) enviado(s) com sucesso aos seus respectivos estaleiros!";
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = "Erro ao processar envio em lote: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
