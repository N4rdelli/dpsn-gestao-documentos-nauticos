using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;


namespace dpsn_gestao_documentos_nauticos.Models
{
    public class Estaleiro: ApplicationUser
    {

        [Required(ErrorMessage = "O nome fantasia é obrigatório.")]
        public string NomeFantasia { get; set; }
        [Required(ErrorMessage = "A razão social é obrigatória.")]
        public string RazaoSocial { get; set; }
        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [MaxLength(14, ErrorMessage = "CNPJ inválido.")]
        public string Cnpj { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve conter no mínimo 6 caracteres.")]
        // Essa diretiva garante que senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial.
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&].+$",
            ErrorMessage = "A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial.")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }


    }
}
