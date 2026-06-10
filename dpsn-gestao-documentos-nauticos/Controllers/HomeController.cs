using dpsn_gestao_documentos_nauticos.Data;
using dpsn_gestao_documentos_nauticos.Models;
using dpsn_gestao_documentos_nauticos.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static dpsn_gestao_documentos_nauticos.Models.Documento;

namespace dpsn_gestao_documentos_nauticos.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MongoDbContext _context;

        public HomeController(UserManager<ApplicationUser> userManager, MongoDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = new DashboardViewModel();

            // Carrega dados globais do Mongo para processamento em memória
            List<Documento> documentos = await _context.Documentos.Find(d => true).ToListAsync();
            List<Estaleiro> estaleiros = await _context.Estaleiros.Find(e => true).ToListAsync();
            List<Embarcacao> embarcacoes = await _context.Embarcacoes.Find(em => true).ToListAsync();

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                model.IsEstaleiro = false;

                // Mapeia os Tecnólogos cadastrados no sistema
                List<Tecnologo> tecnologos = await _context.Tecnologos.Find(t => true).ToListAsync();

                // Regra Admin: Alocação correta e isolada de cada contador solicitado
                model.TotalTecnologos = tecnologos.Count;
                model.TotalEstaleiros = estaleiros.Count;
                model.TotalEmbarcoes = examinarTotalEmbarcoesGerais(embarcacoes);
                model.TotalDocumentosSistema = documentos.Count;
                model.TotalDocumentosAssinados = documentos.Count(d => d.Status == StatusDocumento.Assinado);

                // Configurações dos Gráficos do Admin (Controle temporal e de distribuição)
                model.MesesLabels = new List<string> { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
                for (int i = 1; i <= 12; i++)
                {
                    model.HistoricoSeries1.Add(embarcacoes.Count); // Mantém compatibilidade com estrutura original de séries
                    model.HistoricoSeries2.Add(estaleiros.Count);
                }

                // Popula o Gráfico 3 (Volume de Documentos por Estaleiro) para o Admin
                foreach (var est in estaleiros)
                {
                    model.EstaleirosNomes.Add(est.NomeFantasia);
                    model.QuantidadeDocumentosPorEstaleiro.Add(documentos.Count(d => d.Estaleiro.Id == est.Id));
                }
            }
            else if (await _userManager.IsInRoleAsync(user, "Tecnologo"))
            {
                model.IsEstaleiro = false;

                // Regra Tecnólogo: Vê apenas os SEUS estaleiros e o que acontece neles
                var meusEstaleirosIds = estaleiros.Where(e => e.TecnologoId == user.Id).Select(e => e.Id).ToList();

                model.TotalEstaleiros = meusEstaleirosIds.Count;
                model.TotalEmbarcoes = embarcacoes.Count(e => meusEstaleirosIds.Contains(e.EstaleiroId));
                model.TotalDocumentosAssinados = documentos.Count(d => meusEstaleirosIds.Contains(d.Estaleiro.Id) && d.Status == StatusDocumento.Assinado);
                model.TotalAssinaturasPendentes = documentos.Count(d => meusEstaleirosIds.Contains(d.Estaleiro.Id) && d.Status != StatusDocumento.Assinado);

                // Gráfico de distribuição por estaleiro dele
                var meusEstaleiros = estaleiros.Where(e => e.TecnologoId == user.Id).ToList();
                model.EstaleirosNomes = meusEstaleiros.Select(e => e.NomeFantasia).ToList();
                // Popula séries de gráficos específicos baseados nos dados dele...
            }
            else if (await _userManager.IsInRoleAsync(user, "Estaleiro"))
            {
                model.IsEstaleiro = true;

                // Regra Estaleiro: Vê apenas os seus próprios documentos e embarcações
                model.TotalDocumentosAssinados = documentos.Count(d => d.Estaleiro.Id == user.Id && d.Status == StatusDocumento.Assinado);
                model.TotalAssinaturasPendentes = documentos.Count(d => d.Estaleiro.Id == user.Id && d.Status != StatusDocumento.Assinado);
                model.TotalEmbarcoes = embarcacoes.Count(e => e.EstaleiroId == user.Id);

                model.MesesLabels = new List<string> { "Janeiro", "Fevereiro", "Março", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro" };
                for (int i = 1; i <= 12; i++)
                {
                    model.HistoricoSeries1.Add(documentos.Count(d => d.Estaleiro.Id == user.Id && d.Status == StatusDocumento.Assinado && d.DataCriacaoDocumento.Month == i));
                    model.HistoricoSeries2.Add(documentos.Count(d => d.Estaleiro.Id == user.Id && d.Status != StatusDocumento.Assinado && d.DataCriacaoDocumento.Month == i));
                }
            }

            return View(model);
        }

        private long examinarTotalEmbarcoesGerais(List<Embarcacao> embs) => embs.Count;
    }
}