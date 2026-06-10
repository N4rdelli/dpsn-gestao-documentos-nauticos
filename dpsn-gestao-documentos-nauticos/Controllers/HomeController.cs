using dpsn_gestao_documentos_nauticos.Data;
using dpsn_gestao_documentos_nauticos.Models;
using dpsn_gestao_documentos_nauticos.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using static dpsn_gestao_documentos_nauticos.Models.Documento;

namespace dpsn_gestao_documentos_nauticos.Controllers
{
    [Authorize] // Garante que apenas usuários logados acessem
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
            // Obtém o usuário conectado e checa suas Roles
            var user = await _userManager.GetUserAsync(User);
            bool isTecnologoOrAdmin = await _userManager.IsInRoleAsync(user, "Admin") ||
                                     await _userManager.IsInRoleAsync(user, "Tecnologo");

            var model = new DashboardViewModel();
            List<Documento> documentos = new();
            List<Estaleiro> estaleiros = new();
            List<Embarcacao> embarcacoes = new();


            if (isTecnologoOrAdmin)
            {

                model.IsEstaleiro = false;
                
                // Busca todos os estaleiros, documentos e embarcações do banco e adiciona a uma lista.
                documentos = await _context.Documentos.Find(_ => true).ToListAsync();
                estaleiros = await _context.Estaleiros.Find(_ => true).ToListAsync();
                embarcacoes = await _context.Embarcacoes.Find(_ => true).ToListAsync();

                // Consultas para preencher os dados na dashboard
                model.TotalDocumentosAssinados = documentos.Count(d => d.Status == StatusDocumento.Assinado);
                model.TotalAssinaturasPendentes = documentos.Count(d => d.Status != StatusDocumento.Assinado);
                model.TotalPrestesAExpirar = documentos.Count(d => d.DataCriacaoDocumento <= DateTime.UtcNow.AddDays(-25) && d.Status != StatusDocumento.Assinado);
                model.TotalEstaleiros = estaleiros.Count();
                model.TotalEmbarcoes = embarcacoes.Count();

                // Gráfico 2: Evolução Temporal Global (Novas Embarcações vs Novos Estaleiros)
                model.MesesLabels = new List<string> { "Janeiro", "Feveveiro", "Março", "Abril", 
                    "Maio", "junho", "julho", "agosto", "setembro", "outubro", "novembro", "dezembro" };
                for(int i = 0; i < 12; i++)
                {
                    int mes = i + 1;
                    int countEmbarcacoes = embarcacoes.Count(e => e.Data.Month == mes);
                    int countEstaleiros = estaleiros.Count(e => e.DataCadastro.Month == mes);
                    model.HistoricoSeries1.Add(countEmbarcacoes); // Embarcações criadas por mês
                    model.HistoricoSeries2.Add(countEstaleiros); //Estaleiros cadastrados por mês
                }

                // Gráfico 3: Exclusivo do Tecnólogo (Documentos por Estaleiro)
                model.EstaleirosNomes = estaleiros.Select(e => e.NomeFantasia).ToList();
                model.QuantidadeDocumentosPorEstaleiro = documentos.GroupBy(e => e.Estaleiro.Id).Select(g => g.Count()).ToList();
            }
            else
            {
                model.IsEstaleiro = true;
                documentos = await _context.Documentos.Find(_ => true).ToListAsync();
                embarcacoes = await _context.Embarcacoes.Find(_ => true).ToListAsync();

                // O ID do estaleiro é usado para filtrar: user.Id
                // Dados filtrados (Somente o que pertence a este estaleiro específico)
                // Os dados aqui também são fictícios ainda
                model.TotalDocumentosAssinados = documentos.Count(d => d.Estaleiro.Id == user.Id && d.Status == StatusDocumento.Assinado);
                model.TotalAssinaturasPendentes = documentos.Count(d => d.Estaleiro.Id == user.Id && d.Status != StatusDocumento.Assinado);
                model.TotalPrestesAExpirar = documentos.Count(d => d.Estaleiro.Id == user.Id
                                                    && d.DataCriacaoDocumento <= DateTime.UtcNow.AddDays(-25) && d.Status != StatusDocumento.Assinado);
                model.TotalEmbarcoes = embarcacoes.Where(e => e.EstaleiroId == user.Id).Count();

                // Gráfico 2: Evolução de Documentos do próprio Estaleiro (Assinados vs Pendentes)
                model.MesesLabels = new List<string> { "Janeiro", "Feveveiro", "Março", "Abril",
                    "Maio", "junho", "julho", "agosto", "setembro", "outubro", "novembro", "dezembro" };
                for (int i = 0; i < 12; i++)
                {
                    int mes = i + 1;
                    int countAssinados = documentos.Count(d => d.Estaleiro.Id == user.Id && d.Status == StatusDocumento.Assinado && d.DataCriacaoDocumento.Month == mes);
                    int countPendentes = documentos.Count(d => d.Estaleiro.Id == user.Id && d.Status != StatusDocumento.Assinado && d.DataCriacaoDocumento.Month == mes);
                    model.HistoricoSeries1.Add(countAssinados); // Histórico de documentos assinados por ele
                    model.HistoricoSeries2.Add(countPendentes); // Histórico de pendências dele
                }
            }

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}