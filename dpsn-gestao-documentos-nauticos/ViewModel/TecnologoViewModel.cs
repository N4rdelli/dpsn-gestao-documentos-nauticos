using System.ComponentModel.DataAnnotations;

namespace dpsn_gestao_documentos_nauticos.ViewModel
{
    public class TecnologoViewModel
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha provisória é obrigatória.")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }
    }
}