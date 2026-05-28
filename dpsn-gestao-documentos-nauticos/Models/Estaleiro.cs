using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;


namespace dpsn_gestao_documentos_nauticos.Models
{
    public class Estaleiro: ApplicationUser
    {
        // O atributo do Id é herdado da classe ApplicationUser.

        [Required(ErrorMessage = "O nome fantasia é obrigatório.")]
        public string NomeFantasia { get; set; }
        [Required(ErrorMessage = "A razão social é obrigatória.")]
        public string RazaoSocial { get; set; }
        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [MaxLength(14, ErrorMessage = "CNPJ inválido.")]
        public string Cnpj { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Digite sua senha.")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string Telefone { get; set; }
        public Endereco Endereco { get; set; }
    }
}
