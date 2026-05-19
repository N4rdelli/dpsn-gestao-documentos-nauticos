using dpsn_gestao_documentos_nauticos.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace dpsn_gestao_documentos_nauticos.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings, IMongoClient mongoClient)
        {
            _database = mongoClient.GetDatabase(settings.Value.DatabaseName);
        }

        // Exemplo de coleção
        //public IMongoCollection<Estaleiro> Estaleiros =>
        //    _database.GetCollection<Estaleiro>("Estaleiros");

        // Coleção Users 
        public IMongoCollection<Estaleiro> Estaleiros
        {
            get
            {
                //  Pega a coleção de usuários 
                var userCollection = _database.GetCollection<ApplicationUser>("Users");

                // O OfType<Estaleiro>() filtra automaticamente pelo campo "_t": "Estaleiro" 
                return userCollection.OfType<Estaleiro>();
            }
        }

    }
}
