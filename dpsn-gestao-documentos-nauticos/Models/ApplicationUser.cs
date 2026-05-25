using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;
namespace dpsn_gestao_documentos_nauticos.Models
{
    [CollectionName("Users")]
    // Faz com que a classe AplicationUser ignore os campos extras que estão na coleção. Como os nomes dos estaleiros e seus outros campos.
    [BsonIgnoreExtraElements]
    public class ApplicationUser: MongoDbIdentityUser
    {
    }
}
