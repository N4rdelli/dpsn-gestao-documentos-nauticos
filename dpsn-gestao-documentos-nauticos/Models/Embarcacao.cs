using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.ComponentModel.DataAnnotations;
namespace dpsn_gestao_documentos_nauticos.Models
{
    public class Embarcacao
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? Id { get; set; }
        public int IdEmbarcacao { get; set; } 
        public int? EstaleiroId { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public string? Nome { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public double ComprimentoTotal { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public double BocaMoldada { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public double PontalMoldado { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public double CaladoMaximo { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public double CaladoLeve { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public double ArqueacaoBruta { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public double ArqueacaoLiquida { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public double Tpb { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public double Contorno { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public double Lastro { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public string AreaNavegacaoTipoServico { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public string TipoEmbarcacao { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public string MaterialCasco { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public int MotorizacaoMax { get; set; } 
        public int? MotorizacaoMin { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public int NumTripulantes { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public int NumPassageiros { get; set; }
        [Required(ErrorMessage = "O campo é Obrigatório")]
        public string? NumInscricao { get; set; } 
    }
}
