using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Simpl.Snippets.Service.DataAccess.Models;

namespace Simpl.Snippets.Service.DataAccess.Abstract
{
    public abstract class AbstractMongoDbRepository
    {
        protected IMongoCollection<TSnippet> Collection { get; }

        protected AbstractMongoDbRepository(IOptions<SnippetDatabaseSettings> snippetMongoDbSettings)
        {
            var settings = snippetMongoDbSettings.Value;

            var mongoClient = new MongoClient(settings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(settings.DatabaseName);
            Collection = mongoDatabase.GetCollection<TSnippet>(settings.CollectionName);
        }
    }
}
