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

        // READ: Lista todas as embarcações
        public async Task<IActionResult> Index()
        {
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User?.IsInRole("Admin") ?? false;

            // Se for admin, lista tudo; se for estaleiro lista apenas as embarcações do usuário
            var filter = isAdmin ? Builders<Embarcacao>.Filter.Empty : Builders<Embarcacao>.Filter.Eq(e => e.EstaleiroId, currentUserId);
            var embarcacoes = await _embarcacoesCollection.Find(filter).ToListAsync();

            var estaleiros = await _estaleirosCollection.Find(_ => true).ToListAsync();
            var estaleiroDict = estaleiros.ToDictionary(e => e.Id, e => e.NomeFantasia);

            var viewModelList = embarcacoes.Select(e => new EmbarcacaoViewModel
            {
                IdEmbarcacao = e.IdEmbarcacao,
                Nome = e.Nome,
                TipoEmbarcacao = e.TipoEmbarcacao,
                AreaNavegacaoTipoServico = e.AreaNavegacaoTipoServico,
                ComprimentoTotal = e.ComprimentoTotal,
                NomeEstaleiro = estaleiroDict.ContainsKey(e.EstaleiroId) ? estaleiroDict[e.EstaleiroId] : "Estaleiro Não Encontrado"
            }).ToList();

            return View(viewModelList);
        }

        // CREATE: Get Formulário
        public async Task<IActionResult> Create()
        {
            // CORRIGIDO: Seed preventivo adaptado para as propriedades da sua Model
            if (await _estaleirosCollection.CountDocumentsAsync(_ => true) == 0)
            {
                await _estaleirosCollection.InsertOneAsync(new Estaleiro
                {
                    NomeFantasia = "Estaleiro Naval Central",
                    RazaoSocial = "Central Engenharia Naval S.A.",
                    Cnpj = "12345678000199",
                    Telefone = "1399999999",
                    Email = "contato@central.com",
                    Senha = "Senha@Forte123" // Passa na sua validação Regex
                });
            }

            var model = new EmbarcacaoViewModel();

            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User?.IsInRole("Admin") ?? false;

            // Se não for admin, predefina EstaleiroId para o usuário atual
            if (!isAdmin && !string.IsNullOrEmpty(currentUserId))
            {
                model.EstaleiroId = currentUserId;
            }

            await CarregarEstaleirosDropdown(model);
            return View(model);
        }

        // CREATE: Post Ação
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmbarcacaoViewModel model)
        {
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User?.IsInRole("Admin") ?? false;

            // Se não for admin forçe o EstaleiroId para o usuário autenticado
            if (!isAdmin)
            {
                if (!string.IsNullOrEmpty(currentUserId))
                {
                    model.EstaleiroId = currentUserId;
                }
                else
                {
                    // Usuário não autenticado: para permitir teste local, atribui o primeiro estaleiro disponível
                    var primeiro = await _estaleirosCollection.Find(_ => true).FirstOrDefaultAsync();
                    if (primeiro == null)
                    {
                        // Seed rápido para permitir teste
                        primeiro = new Estaleiro
                        {
                            NomeFantasia = "Estaleiro de Teste",
                            RazaoSocial = "Teste Ltda",
                            Cnpj = "00000000000000",
                            Telefone = "000000000",
                            Email = "teste@local",
                            Senha = "Senha@123"
                        };
                        await _estaleirosCollection.InsertOneAsync(primeiro);
                    }
                    model.EstaleiroId = primeiro.Id;
                    TempData["MensagemAviso"] = "Usuário não autenticado: usando estaleiro de teste para persistir (apenas para teste).";
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var embarcacao = new Embarcacao
                    {
                        EstaleiroId = model.EstaleiroId,
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
                        MotorizacaoMin = model.MotorizacaoMin,
                        Data = DateTime.UtcNow
                    };

                    await _embarcacoesCollection.InsertOneAsync(embarcacao);
                    TempData["MensagemSucesso"] = "Embarcação criada com sucesso.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao inserir embarcação: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao salvar a embarcação. Verifique o log do servidor.");
                }
            }

            await CarregarEstaleirosDropdown(model);
            return View(model);
        }

        // UPDATE: Get Formulário
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var e = await _embarcacoesCollection.Find(x => x.IdEmbarcacao == id).FirstOrDefaultAsync();
            if (e == null) return NotFound();

            // Só o estaleiro dono ou admin pode editar
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User?.IsInRole("Admin") ?? false;
            if (!isAdmin && e.EstaleiroId != currentUserId) return Forbid();

            var model = new EmbarcacaoViewModel
            {
                IdEmbarcacao = e.IdEmbarcacao,
                EstaleiroId = e.EstaleiroId,
                Nome = e.Nome,
                ComprimentoTotal = e.ComprimentoTotal,
                BocaMoldada = e.BocaMoldada,
                PontalMoldado = e.PontalMoldado,
                CaladoMaximo = e.CaladoMaximo,
                CaladoLeve = e.CaladoLeve,
                ArqueacaoBruta = e.ArqueacaoBruta,
                ArqueacaoLiquida = e.ArqueacaoLiquida,
                Tpb = e.Tpb,
                Contorno = e.Contorno,
                Lastro = e.Lastro,
                AreaNavegacaoTipoServico = e.AreaNavegacaoTipoServico,
                TipoEmbarcacao = e.TipoEmbarcacao,
                MaterialCasco = e.MaterialCasco,
                MotorizacaoMax = e.MotorizacaoMax,
                MotorizacaoMin = e.MotorizacaoMin
            };

            await CarregarEstaleirosDropdown(model);
            return View(model);
        }

        // UPDATE: Post Ação
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, EmbarcacaoViewModel model)
        {
            if (id != model.IdEmbarcacao) return NotFound();
            // Ownership check: only owner or admin can update
            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User?.IsInRole("Admin") ?? false;

            var existing = await _embarcacoesCollection.Find(x => x.IdEmbarcacao == id).FirstOrDefaultAsync();
            if (existing == null) return NotFound();
            if (!isAdmin && existing.EstaleiroId != currentUserId) return Forbid();

            if (ModelState.IsValid)
            {
                var filter = Builders<Embarcacao>.Filter.Eq(x => x.IdEmbarcacao, id);
                var update = Builders<Embarcacao>.Update
                    .Set(x => x.EstaleiroId, model.EstaleiroId)
                    .Set(x => x.Nome, model.Nome)
                    .Set(x => x.ComprimentoTotal, model.ComprimentoTotal)
                    .Set(x => x.BocaMoldada, model.BocaMoldada)
                    .Set(x => x.PontalMoldado, model.PontalMoldado)
                    .Set(x => x.CaladoMaximo, model.CaladoMaximo)
                    .Set(x => x.CaladoLeve, model.CaladoLeve)
                    .Set(x => x.ArqueacaoBruta, model.ArqueacaoBruta)
                    .Set(x => x.ArqueacaoLiquida, model.ArqueacaoLiquida)
                    .Set(x => x.Tpb, model.Tpb)
                    .Set(x => x.Contorno, model.Contorno)
                    .Set(x => x.Lastro, model.Lastro)
                    .Set(x => x.AreaNavegacaoTipoServico, model.AreaNavegacaoTipoServico)
                    .Set(x => x.TipoEmbarcacao, model.TipoEmbarcacao)
                    .Set(x => x.MaterialCasco, model.MaterialCasco)
                    .Set(x => x.MotorizacaoMax, model.MotorizacaoMax)
                    .Set(x => x.MotorizacaoMin, model.MotorizacaoMin);

                await _embarcacoesCollection.UpdateOneAsync(filter, update);
                return RedirectToAction(nameof(Index));
            }

            await CarregarEstaleirosDropdown(model);
            return View(model);
        }

        // DELETE: Get Confirmação
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var e = await _embarcacoesCollection.Find(x => x.IdEmbarcacao == id).FirstOrDefaultAsync();
            if (e == null) return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User?.IsInRole("Admin") ?? false;
            if (!isAdmin && e.EstaleiroId != currentUserId) return Forbid();

            return View(e);
        }

        // DELETE: Post Ação de Exclusão definitiva
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var existing = await _embarcacoesCollection.Find(x => x.IdEmbarcacao == id).FirstOrDefaultAsync();
            if (existing == null) return NotFound();

            var currentUserId = _userManager.GetUserId(User);
            var isAdmin = User?.IsInRole("Admin") ?? false;
            if (!isAdmin && existing.EstaleiroId != currentUserId) return Forbid();

            await _embarcacoesCollection.DeleteOneAsync(x => x.IdEmbarcacao == id);
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