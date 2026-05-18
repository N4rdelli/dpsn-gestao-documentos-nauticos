using System.ComponentModel.DataAnnotations;

namespace dpsn_gestao_documentos_nauticos.Models
{
    public class Endereco
    {
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [MaxLength(8, ErrorMessage = "CEP inválido.")]
        public string? Cep { get; set; }
        [Required(ErrorMessage = "O logradouro é obrigatório.")]
        public string? Logradouro { get; set; }
        [Required(ErrorMessage = "O número é obrigatório.") ]
        public string? Numero { get; set; }
        public string? Complemento { get; set; }
        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string? Bairro { get; set; }
        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string? Cidade { get; set; }
        [Required(ErrorMessage = "O estado é obrigatório.")]
        public string? Estado { get; set; }
        
    }
}
