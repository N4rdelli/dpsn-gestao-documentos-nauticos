using System.Collections.Generic;

namespace dpsn_gestao_documentos_nauticos.ViewModel
{
    public class DashboardViewModel
    {
        // Controle de Perfil na View
        public bool IsEstaleiro { get; set; }

        // Contadores dos Cards Superiores (Dinâmicos por perfil)
        public long TotalDocumentosAssinados { get; set; }
        public long TotalAssinaturasPendentes { get; set; }
        public long TotalPrestesAExpirar { get; set; }
        public long TotalEstaleiros { get; set; } // Ocultado ou alterado se for Estaleiro
        public long TotalEmbarcoes { get; set; }

        // Gráfico 1: Status de Documentos (Geral ou filtrado pelo Estaleiro)
        // Usa os contadores acima diretamente no JS

        // Gráfico 2: Evolução Temporal (Geral ou apenas documentos do próprio Estaleiro)
        public List<string> MesesLabels { get; set; } = new List<string>();
        public List<int> HistoricoSeries1 { get; set; } = new List<int>(); // Tecnólogo: Embarcações | Estaleiro: Documentos Assinados
        public List<int> HistoricoSeries2 { get; set; } = new List<int>(); // Tecnólogo: Estaleiros | Estaleiro: Documentos Pendentes

        // Gráfico 3: Apenas para Tecnólogo/Admin
        public List<string> EstaleirosNomes { get; set; } = new List<string>();
        public List<int> QuantidadeDocumentosPorEstaleiro { get; set; } = new List<int>();
    }
}