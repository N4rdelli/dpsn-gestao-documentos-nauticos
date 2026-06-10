using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace dpsn_gestao_documentos_nauticos.Models
{
    public class Tecnologo : ApplicationUser
    {
        [Required(ErrorMessage = "O nome completo é obrigatório.")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        public string Cpf { get; set; }

        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    }
}