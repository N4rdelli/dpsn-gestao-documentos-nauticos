using dpsn_gestao_documentos_nauticos.Models;
using dpsn_gestao_documentos_nauticos.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using dpsn_gestao_documentos_nauticos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dpsn_gestao_documentos_nauticos.Controllers
{
    public class EmbarcacoesController : Controller
    {
        private readonly IMongoCollection<Embarcacao> _embarcacoesCollection;
        private readonly IMongoCollection<Estaleiro> _estaleirosCollection;
        private readonly MongoDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public EmbarcacoesController(MongoDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _embarcacoesCollection = _context.Embarcacoes;
            _estaleirosCollection = _context.Estaleiros;
            _userManager = userManager;
        }

        // GET/Embarcacoes
        // Lista todas as embarcações
        public async Task<IActionResult> Index(string termoBusca, string estaleiroId)
        {
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = (User?.IsInRole("Admin") ?? false) || (User?.IsInRole("Tecnologo") ?? false);

            // 1. Buscar todos os estaleiros para popular o filtro do Admin/Tecnólogo
            var todosEstaleiros = await _estaleirosCollection.Find(_ => true).ToListAsync();
            ViewBag.EstaleirosFiltro = new SelectList(todosEstaleiros, "Id", "NomeFantasia", estaleiroId);

            // Guardar os valores atuais na view para manter os inputs preenchidos
            ViewData["CurrentTermo"] = termoBusca;
            ViewData["CurrentEstaleiro"] = estaleiroId;

            List<Embarcacao> embarcacoes;

            // 2. Aplicar regras de visibilidade de dados por Role
            if (isAdmin)
            {
                // Admin vê tudo por padrão
                embarcacoes = await _embarcacoesCollection.Find(_ => true).ToListAsync();
            }
            else
            {
                // Estaleiro vê apenas as próprias embarcações
                embarcacoes = await _embarcacoesCollection.Find(x => x.EstaleiroId == currentUserId).ToListAsync();
            }

            // 3. Mapear para a ViewModel injetando o nome do estaleiro correspondente
            var listaViewModel = new List<EmbarcacaoViewModel>();
            foreach (var emb in embarcacoes)
            {
                var est = todosEstaleiros.FirstOrDefault(e => e.Id == emb.EstaleiroId);

                listaViewModel.Add(new EmbarcacaoViewModel
                {
                    IdEmbarcacao = emb.IdEmbarcacao,
                    EstaleiroId = emb.EstaleiroId,
                    NomeEstaleiro = est?.NomeFantasia ?? "Estaleiro Não Encontrado",
                    Nome = emb.Nome,
                    TipoEmbarcacao = emb.TipoEmbarcacao,
                    MaterialCasco = emb.MaterialCasco,
                    ComprimentoTotal = emb.ComprimentoTotal,
                    BocaMoldada = emb.BocaMoldada,
                    PontalMoldado = emb.PontalMoldado,
                    CaladoMaximo = emb.CaladoMaximo,
                    CaladoLeve = emb.CaladoLeve,
                    ArqueacaoBruta = emb.ArqueacaoBruta,
                    ArqueacaoLiquida = emb.ArqueacaoLiquida,
                    Tpb = emb.Tpb,
                    Contorno = emb.Contorno,
                    Lastro = emb.Lastro,
                    AreaNavegacaoTipoServico = emb.AreaNavegacaoTipoServico,
                    MotorizacaoMax = emb.MotorizacaoMax,
                    MotorizacaoMin = (int)emb.MotorizacaoMin,
                    Tripulantes = emb.Tripulantes,
                    Passageiros = emb.Passageiros,

                });
            }

            // 4. Aplicar Filtros de Backend (Termos: Nome da Embarcação ou Nome do Estaleiro)
            if (!string.IsNullOrEmpty(termoBusca))
            {
                termoBusca = termoBusca.Trim().ToLower();
                listaViewModel = listaViewModel.Where(x =>
                    x.Nome.ToLower().Contains(termoBusca) ||
                    x.NomeEstaleiro.ToLower().Contains(termoBusca)
                ).ToList();
            }

            // 5. Aplicar Filtro Avançado por Dropdown de Estaleiro (Apenas Admin/Tecnólogo)
            if (isAdmin && !string.IsNullOrEmpty(estaleiroId))
            {
                listaViewModel = listaViewModel.Where(x => x.EstaleiroId == estaleiroId).ToList();
            }

            return View(listaViewModel);
        }

        // GET: Embarcacoes/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new EmbarcacaoViewModel();

            // Se o usuário logado for um Estaleiro, pré-vincula o GUID dele
            var currentUserId = _userManager.GetUserId(User);
            var isEstaleiro = User?.IsInRole("Estaleiro") ?? false;

            if (isEstaleiro && !string.IsNullOrEmpty(currentUserId))
            {
                model.EstaleiroId = currentUserId;
            }

            await CarregarEstaleirosDropdown(model);
            return View(model);
        }

        // POST: Embarcacoes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmbarcacaoViewModel model)
        {
            // Limpa a validação das propriedades que não vêm do formulário HTML
            ModelState.Remove(nameof(model.IdEmbarcacao));
            ModelState.Remove(nameof(model.NomeEstaleiro));
            ModelState.Remove(nameof(model.EstaleirosDisponiveis));

            // Segurança de Back-end: Força o ID do próprio usuário logado se ele for um Estaleiro
            var currentUserId = _userManager.GetUserId(User);
            var isEstaleiro = User?.IsInRole("Estaleiro") ?? false;

            if (isEstaleiro && !string.IsNullOrEmpty(currentUserId))
            {
                model.EstaleiroId = currentUserId;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Mapeia os dados da ViewModel para a Entidade do MongoDB
                    var novaEmbarcacao = new Embarcacao
                    {
                        EstaleiroId = model.EstaleiroId, // Agora salva o GUID de 36 caracteres diretamente
                        Nome = model.Nome,
                        ComprimentoTotal = model.ComprimentoTotal,
                        BocaMoldada = model.BocaMoldada,
                        PontalMoldado = model.PontalMoldado,
                        CaladoMaximo = model.CaladoMaximo,
                        CaladoLeve = model.CaladoLeve,
                        ArqueacaoBruta = model.ArqueacaoBruta,
                        ArqueacaoLiquida = model.ArqueacaoLiquida,
                        Tpb = model.Tpb,
                        Contorno = model.Contorno,
                        Lastro = model.Lastro,
                        AreaNavegacaoTipoServico = model.AreaNavegacaoTipoServico,
                        TipoEmbarcacao = model.TipoEmbarcacao,
                        MaterialCasco = model.MaterialCasco,
                        MotorizacaoMax = model.MotorizacaoMax,
                        MotorizacaoMin = (int)model.MotorizacaoMin,
                        Tripulantes = model.Tripulantes,
                        Passageiros = model.Passageiros,
                        // PotenciaTotalHp e ComprimentoRegra removidos com sucesso
                    };

                    // Insere no banco MongoDB sem restrições de formatação hexadecimal
                    await _embarcacoesCollection.InsertOneAsync(novaEmbarcacao);

                    TempData["MensagemSucesso"] = "Embarcação registrada com sucesso no sistema!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro interno ao salvar no MongoDB: {ex.Message}");
                }
            }

            // Se houver erros, recarrega a estrutura do dropdown e exibe as validações
            await CarregarEstaleirosDropdown(model);
            return View(model);
        }

        // GET: Embarcacoes/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            // Busca a embarcação diretamente no MongoDB
            var embarcacao = await _embarcacoesCollection.Find(x => x.IdEmbarcacao == id).FirstOrDefaultAsync();
            if (embarcacao == null) return NotFound();

            // Segurança: Se o usuário for Estaleiro, ele só pode editar as próprias embarcações
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User?.IsInRole("Admin") ?? false;
            if (!isAdmin && embarcacao.EstaleiroId != currentUserId)
            {
                return Forbid();
            }

            // Mapeia a Entidade do banco para a ViewModel da tela
            var model = new EmbarcacaoViewModel
            {
                IdEmbarcacao = embarcacao.IdEmbarcacao,
                EstaleiroId = embarcacao.EstaleiroId,
                Nome = embarcacao.Nome,
                ComprimentoTotal = embarcacao.ComprimentoTotal,
                BocaMoldada = embarcacao.BocaMoldada,
                PontalMoldado = embarcacao.PontalMoldado,
                CaladoMaximo = embarcacao.CaladoMaximo,
                CaladoLeve = embarcacao.CaladoLeve,
                ArqueacaoBruta = embarcacao.ArqueacaoBruta,
                ArqueacaoLiquida = embarcacao.ArqueacaoLiquida,
                Tpb = embarcacao.Tpb,
                Contorno = embarcacao.Contorno,
                Lastro = embarcacao.Lastro,
                AreaNavegacaoTipoServico = embarcacao.AreaNavegacaoTipoServico,
                TipoEmbarcacao = embarcacao.TipoEmbarcacao,
                MaterialCasco = embarcacao.MaterialCasco,
                MotorizacaoMax = embarcacao.MotorizacaoMax,
                MotorizacaoMin = (int)embarcacao.MotorizacaoMin,
                Tripulantes = embarcacao.Tripulantes,
                Passageiros = embarcacao.Passageiros,
            };

            await CarregarEstaleirosDropdown(model);
            return View(model);
        }

        // POST: Embarcacoes/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EmbarcacaoViewModel model)
        {
            if (id != model.IdEmbarcacao) return NotFound();

            // CORREÇÃO CENTRAL: Remove do validador os campos que não vêm editados do formulário HTML
            ModelState.Remove(nameof(model.NomeEstaleiro));
            ModelState.Remove(nameof(model.EstaleirosDisponiveis));

            // Segurança de Back-end: Se for um usuário de Estaleiro, força o ID dele como dono
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User?.IsInRole("Admin") ?? false;

            if (!isAdmin && !string.IsNullOrEmpty(currentUserId))
            {
                model.EstaleiroId = currentUserId;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Busca o registro atual antes de atualizar para garantir que ele existe e pertence ao usuário
                    var existing = await _embarcacoesCollection.Find(x => x.IdEmbarcacao == id).FirstOrDefaultAsync();
                    if (existing == null) return NotFound();
                    if (!isAdmin && existing.EstaleiroId != currentUserId) return Forbid();

                    // Mapeia as alterações da ViewModel de volta para o objeto de domínio do MongoDB
                    existing.Nome = model.Nome;
                    existing.EstaleiroId = model.EstaleiroId; // Mantém ou atualiza o GUID com segurança
                    existing.ComprimentoTotal = model.ComprimentoTotal;
                    existing.BocaMoldada = model.BocaMoldada;
                    existing.PontalMoldado = model.PontalMoldado;
                    existing.CaladoMaximo = model.CaladoMaximo;
                    existing.CaladoLeve = model.CaladoLeve;
                    existing.ArqueacaoBruta = model.ArqueacaoBruta;
                    existing.ArqueacaoLiquida = model.ArqueacaoLiquida;
                    existing.Tpb = model.Tpb;
                    existing.Contorno = model.Contorno;
                    existing.Lastro = model.Lastro;
                    existing.AreaNavegacaoTipoServico = model.AreaNavegacaoTipoServico;
                    existing.TipoEmbarcacao = model.TipoEmbarcacao;
                    existing.MaterialCasco = model.MaterialCasco;
                    existing.MotorizacaoMax = model.MotorizacaoMax;
                    existing.MotorizacaoMin = (int)model.MotorizacaoMin;
                    existing.Tripulantes = model.Tripulantes;
                    existing.Passageiros = model.Passageiros;
                    

                    // Executa a substituição do documento antigo pelo atualizado no MongoDB
                    var result = await _embarcacoesCollection.ReplaceOneAsync(x => x.IdEmbarcacao == id, existing);

                    if (result.ModifiedCount > 0 || result.MatchedCount > 0)
                    {
                        TempData["MensagemSucesso"] = "Alterações da embarcação salvas com sucesso!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Nenhuma alteração foi detectada ou modificada no banco.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Erro interno ao atualizar no MongoDB: {ex.Message}");
                }
            }

            // Se falhar a validação ou der erro, recarrega a estrutura do dropdown e exibe os alertas na View
            await CarregarEstaleirosDropdown(model);
            return View(model);
        }

        // GET: Embarcacoes/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            // 1. Busca a embarcação diretamente no MongoDB
            var embarcacao = await _embarcacoesCollection.Find(x => x.IdEmbarcacao == id).FirstOrDefaultAsync();
            if (embarcacao == null) return NotFound();

            // 2. Trava de Segurança: Se o usuário logado for um Estaleiro, ele só pode ver os detalhes das suas próprias embarcações
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User?.IsInRole("Admin") ?? false;
            if (!isAdmin && embarcacao.EstaleiroId != currentUserId)
            {
                return Forbid(); // Retorna o erro 403 de Acesso Negado
            }

            // 3. Cria a ViewModel mapeando os dados técnicos da entidade do banco
            var model = new EmbarcacaoViewModel
            {
                IdEmbarcacao = embarcacao.IdEmbarcacao,
                EstaleiroId = embarcacao.EstaleiroId,
                Nome = embarcacao.Nome,
                ComprimentoTotal = embarcacao.ComprimentoTotal,
                BocaMoldada = embarcacao.BocaMoldada,
                PontalMoldado = embarcacao.PontalMoldado,
                CaladoMaximo = embarcacao.CaladoMaximo,
                CaladoLeve = embarcacao.CaladoLeve,
                ArqueacaoBruta = embarcacao.ArqueacaoBruta,
                ArqueacaoLiquida = embarcacao.ArqueacaoLiquida,
                Tpb = embarcacao.Tpb,
                Contorno = embarcacao.Contorno,
                Lastro = embarcacao.Lastro,
                AreaNavegacaoTipoServico = embarcacao.AreaNavegacaoTipoServico,
                TipoEmbarcacao = embarcacao.TipoEmbarcacao,
                MaterialCasco = embarcacao.MaterialCasco,
                MotorizacaoMax = embarcacao.MotorizacaoMax,
                MotorizacaoMin = (int)embarcacao.MotorizacaoMin,
                Tripulantes = embarcacao.Tripulantes,
                Passageiros = embarcacao.Passageiros,

            };

            // 4. Busca o Nome Fantasia do estaleiro para exibir na tela de detalhes
            if (!string.IsNullOrEmpty(model.EstaleiroId))
            {
                var estaleiro = await _estaleirosCollection.Find(e => e.Id == model.EstaleiroId).FirstOrDefaultAsync();
                model.NomeEstaleiro = estaleiro != null ? estaleiro.NomeFantasia : "Estaleiro Não Localizado";
            }
            else
            {
                model.NomeEstaleiro = "Não vinculado";
            }

            return View(model);
        }

        // GET: Embarcacoes/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            // Busca a embarcação no MongoDB
            var embarcacao = await _embarcacoesCollection.Find(x => x.IdEmbarcacao == id).FirstOrDefaultAsync();
            if (embarcacao == null) return NotFound();

            // Segurança: Se for Estaleiro, impede de visualizar/deletar dados de outros
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User?.IsInRole("Admin") ?? false;
            if (!isAdmin && embarcacao.EstaleiroId != currentUserId)
            {
                return Forbid();
            }

            // Mapeia para a ViewModel (removendo os campos inexistentes)
            var model = new EmbarcacaoViewModel
            {
                IdEmbarcacao = embarcacao.IdEmbarcacao,
                EstaleiroId = embarcacao.EstaleiroId,
                Nome = embarcacao.Nome,
                ComprimentoTotal = embarcacao.ComprimentoTotal,
                BocaMoldada = embarcacao.BocaMoldada,
                PontalMoldado = embarcacao.PontalMoldado,
                CaladoMaximo = embarcacao.CaladoMaximo,
                CaladoLeve = embarcacao.CaladoLeve,
                ArqueacaoBruta = embarcacao.ArqueacaoBruta,
                ArqueacaoLiquida = embarcacao.ArqueacaoLiquida,
                Tpb = embarcacao.Tpb,
                Contorno = embarcacao.Contorno,
                Lastro = embarcacao.Lastro,
                AreaNavegacaoTipoServico = embarcacao.AreaNavegacaoTipoServico,
                TipoEmbarcacao = embarcacao.TipoEmbarcacao,
                MaterialCasco = embarcacao.MaterialCasco,
                MotorizacaoMax = embarcacao.MotorizacaoMax,
                MotorizacaoMin = (int)embarcacao.MotorizacaoMin,
                Tripulantes = embarcacao.Tripulantes,
                Passageiros = embarcacao.Passageiros,
            };

            // Busca o nome do estaleiro apenas para exibição amigável na tela de confirmação
            if (!string.IsNullOrEmpty(model.EstaleiroId))
            {
                var estaleiro = await _estaleirosCollection.Find(e => e.Id == model.EstaleiroId).FirstOrDefaultAsync();
                model.NomeEstaleiro = estaleiro != null ? estaleiro.NomeFantasia : "Estaleiro Não Localizado";
            }

            return View(model);
        }

        // POST: Embarcacoes/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            // Busca o registro atual para validar a posse antes de apagar de vez
            var existing = await _embarcacoesCollection.Find(x => x.IdEmbarcacao == id).FirstOrDefaultAsync();
            if (existing == null) return NotFound();

            // Validação de segurança baseada no GUID salvo no banco
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User?.IsInRole("Admin") ?? false;
            if (!isAdmin && existing.EstaleiroId != currentUserId)
            {
                return Forbid();
            }

            // Remove o documento do cluster MongoDB
            await _embarcacoesCollection.DeleteOneAsync(x => x.IdEmbarcacao == id);

            TempData["MensagemSucesso"] = "Embarcação removida com sucesso do sistema.";
            return RedirectToAction(nameof(Index));
        }

        //Carrega o Dropdown vinculando 'Id' ao text 'NomeFantasia'
        private async Task CarregarEstaleirosDropdown(EmbarcacaoViewModel model)
        {
            var estaleiros = await _estaleirosCollection.Find(_ => true).ToListAsync();

            //Se o usuário não for admin, limitar ao próprio estaleiro (garantir que o option exista)
            var isAdmin = User?.IsInRole("Admin") ?? false;
            var currentUserId = _userManager.GetUserId(User);

            if (!isAdmin && !string.IsNullOrEmpty(currentUserId))
            {
                estaleiros = estaleiros.Where(e => e.Id == currentUserId).ToList();
            }

            model.EstaleirosDisponiveis = estaleiros.Select(e => new SelectListItem
            {
                Value = e.Id,
                Text = e.NomeFantasia
            }).ToList();
        }
    }
}