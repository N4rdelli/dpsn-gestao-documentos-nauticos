using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace dpsn_gestao_documentos_nauticos.Models
{
    public class Embarcacao
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdEmbarcacao { get; set; }

        [BsonElement("estaleiro_id")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string EstaleiroId { get; set; } // Guarda o "Id" string vindo do ApplicationUser

        public string Nome { get; set; }
        public decimal ComprimentoTotal { get; set; }
        public decimal BocaMoldada { get; set; }
        public decimal PontalMoldado { get; set; }
        public decimal CaladoMaximo { get; set; }
        public decimal CaladoLeve { get; set; }
        public decimal ArqueacaoBruta { get; set; }
        public decimal ArqueacaoLiquida { get; set; }
        public decimal Tpb { get; set; }
        public decimal Contorno { get; set; }
        public decimal? Lastro { get; set; }
        public string AreaNavegacaoTipoServico { get; set; }
        public string TipoEmbarcacao { get; set; }
        public string MaterialCasco { get; set; }
        public int MotorizacaoMax { get; set; }
        public int MotorizacaoMin { get; set; }
        public DateTime Data { get; set; } = DateTime.UtcNow;
    }
}