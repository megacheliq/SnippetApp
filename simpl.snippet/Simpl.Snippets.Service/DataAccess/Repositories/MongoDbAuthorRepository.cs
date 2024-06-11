using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Simpl.Snippets.Service.DataAccess.Abstract;
using Simpl.Snippets.Service.DataAccess.Models;

namespace Simpl.Snippets.Service.DataAccess.Repositories
{
    public class MongoDbAuthorRepository : AbstractMongoDbRepository, IAuthorRepository
    {
        public MongoDbAuthorRepository(IOptions<SnippetDatabaseSettings> snippetMongoDbSettings)
            : base(snippetMongoDbSettings)
        {
        }

        public async Task<IEnumerable<AuthorDto>> GetAll(Direction direction, CancellationToken cancellationToken = default)
        {
            var result = await Collection
                .AsQueryable()
                .Where(x => x.Direction == direction)
                .Select(x => new AuthorDto {Id = x.AuthorId, Name = x.AuthorName })
                .Distinct()
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
