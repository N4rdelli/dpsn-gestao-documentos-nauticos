
namespace dpsn_gestao_documentos_nauticos.Models
{
    // Cria a classe de configuração que utilizamos no appsettings.json, para que o ASP .NET injete as configurações automaticamente
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
    }
}
