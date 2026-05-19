using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace dpsn_gestao_documentos_nauticos.Models
{
    [CollectionName("Roles")]
    public class ApplicationRole : MongoDbIdentityRole
    {
    }
}
