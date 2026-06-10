using MongoDB.Bson.Serialization.Attributes;

namespace dpsn_gestao_documentos_nauticos.Models
{
    public class Documento
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public Estaleiro Estaleiro { get; set; }
        public Embarcacao Embarcacao { get; set; }
        public Cliente Cliente { get; set; }
        public string NumeroInscricao { get; set; } = "A ser inscrita";
        public DateTime DataCriacaoDocumento { get; set; }
        public DateTime? DataAssinatura { get; set; }

        // Cria o enum que substitui o bool de StatusAssinatura
        public enum StatusDocumento
        {
            RevisaoPendente = 0,
            EmRevisao = 1,
            Assinado = 2
        }
        public StatusDocumento Status { get; set; } = StatusDocumento.RevisaoPendente;
        public string? CaminhoPdfAssinado { get; set; }

    }
}
