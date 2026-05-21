using System.ComponentModel.DataAnnotations;

namespace dpsn_gestao_documentos_nauticos.ViewModel
{
    public class EmbarcacaoViewModel
    {
        public string? Id { get; set; } // ID Hexadecimal do MongoDB

        [Required(ErrorMessage = "O nome da embarcação é obrigatório.")]
        [Display(Name = "Nome da Embarcação")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O número de inscrição é obrigatório.")]
        [Display(Name = "Número de Inscrição")]
        public string NumeroInscricao { get; set; }

        [Required(ErrorMessage = "O tipo de embarcação é obrigatório.")]
        public string Tipo { get; set; }

        [Display(Name = "Estaleiro Responsável")]
        public int? EstaleiroId { get; set; }
    }
}

