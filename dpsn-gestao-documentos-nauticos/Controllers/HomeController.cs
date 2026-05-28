using dpsn_gestao_documentos_nauticos.Data;
using dpsn_gestao_documentos_nauticos.Models;
using dpsn_gestao_documentos_nauticos.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

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

            if (isTecnologoOrAdmin)
            {
                model.IsEstaleiro = false;

                // Dados consolidados, por enquanto fictícios
                // Ainda temos que trocar para os dados reais no banco de dados
                model.TotalDocumentosAssinados = 17;
                model.TotalAssinaturasPendentes = 4;
                model.TotalPrestesAExpirar = 7;
                model.TotalEstaleiros = 4;
                model.TotalEmbarcoes = 23;

                // Gráfico 2: Evolução Temporal Global (Novas Embarcações vs Novos Estaleiros)
                model.MesesLabels = new List<string> { "Dez", "Jan", "Fev", "Mar", "Abr", "Mai" };
                model.HistoricoSeries1 = new List<int> { 12, 15, 18, 19, 21, 23 }; // Embarcações
                model.HistoricoSeries2 = new List<int> { 2, 2, 3, 3, 4, 4 };      // Estaleiros

                // Gráfico 3: Exclusivo do Tecnólogo (Documentos por Estaleiro)
                model.EstaleirosNomes = new List<string> { "Estaleiro Mauá", "Estaleiro Brasa", "Estaleiro Jurong", "Arsenal de Marinha" };
                model.QuantidadeDocumentosPorEstaleiro = new List<int> { 8, 5, 3, 1 };
            }
            else
            {
                model.IsEstaleiro = true;

                // O ID do estaleiro é usado para filtrar: user.Id
                // Dados filtrados (Somente o que pertence a este estaleiro específico)
                // Os dados aqui também são fictícios ainda
                model.TotalDocumentosAssinados = 8;
                model.TotalAssinaturasPendentes = 2;
                model.TotalPrestesAExpirar = 3;
                model.TotalEmbarcoes = 9;      

                // Gráfico 2: Evolução de Documentos do próprio Estaleiro (Assinados vs Pendentes)
                model.MesesLabels = new List<string> { "Dez", "Jan", "Fev", "Mar", "Abr", "Mai" };
                model.HistoricoSeries1 = new List<int> { 2, 4, 5, 5, 7, 8 }; // Histórico de documentos assinados por ele
                model.HistoricoSeries2 = new List<int> { 1, 3, 2, 4, 1, 2 }; // Histórico de pendências dele
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