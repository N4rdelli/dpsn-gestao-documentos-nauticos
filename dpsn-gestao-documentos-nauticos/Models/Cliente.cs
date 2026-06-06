using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace dpsn_gestao_documentos_nauticos.Models
{
    public class Cliente
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [Required(ErrorMessage = "O ano de construção é obrigatório.")]
        public string Ano_contrucao { get; set; }
        [Required(ErrorMessage = "O número do chassi é obrigatório.")]
        public string NumeroChassi { get; set; }
        [Required(ErrorMessage = "O nome do armador é obrigatório.")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "O CPF ou CNPJ é obrigatório.")] 
        public string Cpf_cnpj { get; set; }
        public Endereco Endereco { get; set; }
    }
}
