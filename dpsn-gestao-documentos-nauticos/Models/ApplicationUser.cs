using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;
namespace dpsn_gestao_documentos_nauticos.Models
{
    [CollectionName("Users")]
    public class ApplicationUser: MongoDbIdentityUser
    {
    }
}
