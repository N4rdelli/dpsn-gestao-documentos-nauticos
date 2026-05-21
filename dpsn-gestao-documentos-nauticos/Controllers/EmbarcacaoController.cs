using dpsn_gestao_documentos_nauticos.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dpsn_gestao_documentos_nauticos.Controllers
{
    // CORREÇÃO: Adicionada a herança ": Controller" para habilitar View, User, ModelState, etc.
    public class EmbarcacaoController : Controller
    {
        private readonly IMongoCollection<Embarcacao> _embarcacoesCollection;
        private readonly IMongoCollection<Estaleiro> _estaleirosCollection;

        public EmbarcacaoController(IMongoDatabase database)
        {
            _embarcacoesCollection = database.GetCollection<Embarcacao>("Embarcacoes");
            _estaleirosCollection = database.GetCollection<Estaleiro>("Estaleiros");
        }

        // GET: Embarcacao/Insert
        [HttpGet]
        public async Task<IActionResult> Insert()
        {
            var IsAdmin = User.IsInRole("Administrador");
            var idEstaleiroSessao = User.FindFirst("IdEstaleiro")?.Value;

            if (IsAdmin)
            {
                var estaleiros = await _estaleirosCollection.Find(_ => true).ToListAsync();
                ViewBag.Estaleiros = estaleiros;
                return View("CadastrarEmbarcacaoAdm");
            }

            if (!string.IsNullOrEmpty(idEstaleiroSessao))
            {
                return View("CadastrarEmbarcacaoEstaleiro");
            }

            return Forbid();
        }

        // POST: Embarcacao/Insert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Insert(Embarcacao model, int? estaleiroSelecionado)
        {
            var idEstaleiroSessao = User.FindFirst("IdEstaleiro")?.Value;

            if (string.IsNullOrEmpty(idEstaleiroSessao))
            {
                if (estaleiroSelecionado == null)
                {
                    ModelState.AddModelError("EstaleiroId", "Selecione o estaleiro");
                }
                else
                {
                    model.EstaleiroId = estaleiroSelecionado;
                }
            }
            else
            {
                model.EstaleiroId = int.Parse(idEstaleiroSessao);
            }

            if (ModelState.IsValid)
            {
                await _embarcacoesCollection.InsertOneAsync(model);

                if (!string.IsNullOrEmpty(idEstaleiroSessao))
                    return RedirectToAction("InicioEstaleiro", "Inicio");
                else
                    return RedirectToAction("InicioAdm", "Inicio");
            }

            if (User.IsInRole("Administrador"))
            {
                ViewBag.Estaleiros = await _estaleirosCollection.Find(_ => true).ToListAsync();
                return View("CadastrarEmbarcacaoAdm", model);
            }

            return View("CadastrarEmbarcacaoEstaleiro", model);
        }

        // GET: Embarcacao/Select?id=5
        [HttpGet]
        public async Task<IActionResult> Select(int? id)
        {
            var idEstaleiroSessao = User.FindFirst("IdEstaleiro")?.Value;
            List<Embarcacao> listaEmbarcacoes;

            if (string.IsNullOrEmpty(idEstaleiroSessao))
            {
                if (id.HasValue)
                {
                    listaEmbarcacoes = await _embarcacoesCollection.Find(e => e.EstaleiroId == id).ToListAsync();
                }
                else
                {
                    listaEmbarcacoes = await _embarcacoesCollection.Find(_ => true).ToListAsync();
                }
            }
            else
            {
                int estaleiroId = int.Parse(idEstaleiroSessao);
                listaEmbarcacoes = await _embarcacoesCollection.Find(e => e.EstaleiroId == estaleiroId).ToListAsync();
            }

            return View("TodasEmbarcacoes", listaEmbarcacoes);
        }

        // GET: Embarcacao/Update/64a7b8c...
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var embarcacao = await _embarcacoesCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
            if (embarcacao == null) return NotFound();

            return View("FormUpdateEmbarcacao", embarcacao);
        }

        // POST: Embarcacao/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(string id, Embarcacao model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                await _embarcacoesCollection.ReplaceOneAsync(e => e.Id == id, model);

                var idEstaleiroSessao = User.FindFirst("IdEstaleiro")?.Value;
                if (!string.IsNullOrEmpty(idEstaleiroSessao))
                    return RedirectToAction("InicioEstaleiro", "Inicio");
                else
                    return RedirectToAction("Select", new { id = model.EstaleiroId });
            }

            return View("FormUpdateEmbarcacao", model);
        }

        // POST: Embarcacao/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string idEmbarcacao, int? estaleiroId)
        {
            if (string.IsNullOrEmpty(idEmbarcacao)) return BadRequest();

            await _embarcacoesCollection.DeleteOneAsync(e => e.Id == idEmbarcacao);

            var idEstaleiroSessao = User.FindFirst("IdEstaleiro")?.Value;
            if (!string.IsNullOrEmpty(idEstaleiroSessao))
                return RedirectToAction("InicioEstaleiro", "Inicio");
            else
                return RedirectToAction("Select", new { id = estaleiroId });
        }
    }
}