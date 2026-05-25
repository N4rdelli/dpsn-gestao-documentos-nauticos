using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace dpsn_gestao_documentos_nauticos.ViewModel
{
    public class EmbarcacaoViewModel
    {
        public string IdEmbarcacao { get; set; }

        [Required(ErrorMessage = "O estaleiro é obrigatório.")]
        public string EstaleiroId { get; set; }

        public string NomeEstaleiro { get; set; }

        [Required(ErrorMessage = "O nome da embarcação é obrigatório.")]
        [StringLength(255, ErrorMessage = "O nome não pode exceder 255 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Comprimento Total é obrigatório.")]
        [Range(0.01, 999999.99, ErrorMessage = "Valor inválido.")]
        public decimal ComprimentoTotal { get; set; }

        [Required(ErrorMessage = "Boca Moldada é obrigatória.")]
        [Range(0.01, 999999.99, ErrorMessage = "Valor inválido.")]
        public decimal BocaMoldada { get; set; }

        [Required(ErrorMessage = "Pontal Moldado é obrigatório.")]
        [Range(0.01, 999999.99, ErrorMessage = "Valor inválido.")]
        public decimal PontalMoldado { get; set; }

        [Required(ErrorMessage = "Calado Máximo é obrigatório.")]
        [Range(0.01, 999999.99, ErrorMessage = "Valor inválido.")]
        public decimal CaladoMaximo { get; set; }

        [Required(ErrorMessage = "Calado Leve é obrigatório.")]
        [Range(0.01, 999999.99, ErrorMessage = "Valor inválido.")]
        public decimal CaladoLeve { get; set; }

        [Required(ErrorMessage = "Arqueação Bruta é obrigatória.")]
        [Range(0.01, 999999.99, ErrorMessage = "Valor inválido.")]
        public decimal ArqueacaoBruta { get; set; }

        [Required(ErrorMessage = "Arqueação Líquida é obrigatória.")]
        [Range(0.01, 999999.99, ErrorMessage = "Valor inválido.")]
        public decimal ArqueacaoLiquida { get; set; }

        [Required(ErrorMessage = "TPB é obrigatório.")]
        [Range(0.01, 999999.99, ErrorMessage = "Valor inválido.")]
        public decimal Tpb { get; set; }

        [Required(ErrorMessage = "Contorno é obrigatório.")]
        [Range(0.01, 999999.99, ErrorMessage = "Valor inválido.")]
        public decimal Contorno { get; set; }

        public decimal? Lastro { get; set; }

        [Required(ErrorMessage = "Área de Navegação / Tipo de Serviço é obrigatória.")]
        public string AreaNavegacaoTipoServico { get; set; }

        [Required(ErrorMessage = "Tipo de Embarcação é obrigatório.")]
        public string TipoEmbarcacao { get; set; }

        [Required(ErrorMessage = "Material do Casco é obrigatório.")]
        public string MaterialCasco { get; set; }

        [Required(ErrorMessage = "Motorização Máxima é obrigatória.")]
        public int MotorizacaoMax { get; set; }

        [Required(ErrorMessage = "Motorização Mínima é obrigatória.")]
        public int MotorizacaoMin { get; set; }

        public DateTime Data { get; set; } = DateTime.Now;

        public List<SelectListItem> EstaleirosDisponiveis { get; set; } = new List<SelectListItem>();
    }
}