using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace dpsn_gestao_documentos_nauticos.ViewModel
{
    public class EstaleiroViewModel
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        // Dados do estaleiro
        [Required(ErrorMessage = "O nome fantasia é obrigatório.")]
        public string NomeFantasia { get; set; }
        [Required(ErrorMessage = "A razão social é obrigatória.")]
        public string RazaoSocial { get; set; }
        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        public string Cnpj { get; set; }
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email deve ser válido.")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Digite sua senha.")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "O telefone é obrigatório.")]
        public string Telefone { get; set; }

        // Dados de endereço
        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [MaxLength(8, ErrorMessage = "CEP inválido.")]
        public string Cep { get; set; }
        [Required(ErrorMessage = "O logradouro é obrigatório.")]
        public string Logradouro { get; set; }
        [Required(ErrorMessage = "O número é obrigatório.")]
        public string Numero { get; set; }
        public string? Complemento { get; set; }
        [Required(ErrorMessage = "O bairro é obrigatório.")]
        public string Bairro { get; set; }
        [Required(ErrorMessage = "A cidade é obrigatória.")]
        public string Cidade { get; set; }
        [Required(ErrorMessage = "O estado é obrigatório.")]
        public string Estado { get; set; }

    }
}
