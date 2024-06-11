using FluentValidation;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Simpl.Snippets.Service.DataAccess.Abstract;
using Simpl.Snippets.Service.DataAccess.Models;
using Simpl.Snippets.Service.Domain.Snippet.Models;
using Simpl.Snippets.Service.Domain.Snippet.UseCases.Queries;
using Simpl.Snippets.Service.Exceptions.Models;

namespace Simpl.Snippets.Service.DataAccess.Repositories
{
    public class MongoDbSnippetRepository : AbstractMongoDbRepository, ISnippetRepository
    {

        public MongoDbSnippetRepository(IOptions<SnippetDatabaseSettings> snippetMongoDbSettings) : base(snippetMongoDbSettings)
        {
        }

        public async Task<List<BriefInfoSnippetResponse>> GetFilteredAsync(BriefInfoSnippetsQuery informationSnippets, CancellationToken cancellationToken = default)
        {
            if (informationSnippets is null)
            {
                throw new ArgumentNullException(nameof(informationSnippets));
            }

            var query = Collection.AsQueryable();

            query = query.Where(s => s.Direction == informationSnippets.Direction);

            if (informationSnippets.Level != null)
                query = query.Where(s => s.Level == informationSnippets.Level);

            if (informationSnippets.AuthorId != null)
                query = query.Where(s => s.AuthorId == informationSnippets.AuthorId.Value);

            if (informationSnippets.CreatedDateStart != null)
                query = query.Where(s => s.CreatedDate >= informationSnippets.CreatedDateStart);

            if (informationSnippets.CreatedDateEnd != null)
                query = query.Where(s => s.CreatedDate <= informationSnippets.CreatedDateEnd);

            if (informationSnippets.ModifiedDateStart != null)
                query = query.Where(s => s.ModifiedDate >= informationSnippets.ModifiedDateStart);

            if (informationSnippets.ModifiedDateEnd != null)
                query = query.Where(s => s.ModifiedDate <= informationSnippets.ModifiedDateEnd);

            var snippetCollections = await query
                .Select(collection => new BriefInfoSnippetResponse
                {
                    Id = collection.Id,
                    AuthorId = collection.AuthorId,
                    AuthorName = collection.AuthorName,
                    Theme = collection.Theme,
                    CreatedDate = collection.CreatedDate,
                    ModifiedDate = collection.ModifiedDate
                })
                .OrderByDescending(x => x.ModifiedDate)
                .ToListAsync(cancellationToken);

            return snippetCollections;
        }

        public async Task<FullSnippetInfoResponse> GetByIdOrDefaultAsync(string id, CancellationToken cancellationToken = default)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var result = await Collection
                .AsQueryable()
                .Where(s => s.Id == id)
                .Select(x => new FullSnippetInfoResponse
                {
                    Id = id,
                    Theme = x.Theme,
                    AuthorId = x.AuthorId,
                    AuthorName = x.AuthorName,
                    Direction = x.Direction,
                    Level = x.Level,
                    CodeSnippet = x.CodeSnippet,
                    MainQuestion = x.MainQuestion,
                    Solution = x.Solution,
                    AdditionalQuestions = x.AdditionalQuestions
                })
                .FirstOrDefaultAsync(cancellationToken);

            return result ?? throw new NoDataFoundException($"Не найден снипет с id {id}");
        }

        public async Task<RandomSnippetResponse> GetRandomAsync(RandomSnippetQuery randomSnippet, CancellationToken cancellationToken = default)
        {
            if (randomSnippet is null)
            {
                throw new ArgumentNullException(nameof(randomSnippet));
            }

            var match = new BsonDocument
            {
                { "$match", new BsonDocument
                    {
                        { "Direction", randomSnippet.Direction.ToString() },
                        { "Level", randomSnippet.Level.ToString() }
                    }
                }
            };

            var sample = new BsonDocument { { "$sample", new BsonDocument { { "size", 1 } } } };

            var projection = new BsonDocument
            {
                { "$project", new BsonDocument
                    {
                        { "_id", 0 },
                        { "AuthorName", 1 },
                        { "CodeSnippet", 1 },
                        { "MainQuestion", 1 },
                        { "AdditionalQuestions", 1 }
                    }
                }
            };

            var pipeline = new[] { match, sample, projection };

            var result = await Collection.Aggregate<RandomSnippetResponse>(pipeline, cancellationToken: cancellationToken).FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        public async Task CreateAsync(TSnippet snippetCollection, CancellationToken cancellationToken = default)
        {
            if (snippetCollection is null)
            {
                throw new ArgumentNullException(nameof(snippetCollection));
            }

            snippetCollection.Id = ObjectId.GenerateNewId().ToString();
            await Collection.InsertOneAsync(snippetCollection, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(string id, AddOrUpdateSnippetDto dto, CancellationToken cancellationToken = default)
        {
            if (dto is null)
            {
                throw new ArgumentNullException(nameof(dto));
            }

            var filter = Builders<TSnippet>.Filter.Eq(s => s.Id, id);

            var update = Builders<TSnippet>.Update
                .Set(s => s.Theme, dto.Theme)
                .Set(s => s.Level, dto.Level)
                .Set(s => s.CodeSnippet, dto.CodeSnippet)
                .Set(s => s.MainQuestion, dto.MainQuestion)
                .Set(s => s.Solution, dto.Solution)
                .Set(s => s.AdditionalQuestions, dto.AdditionalQuestions)
                .Set(s => s.ModifiedDate, DateTime.UtcNow);

            var result = await Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);

            if (result.ModifiedCount == 0)
                throw new NoDataFoundException($"Не найден снипет с id {id} для изменения");
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            if (id is null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await Collection.DeleteOneAsync(doc => doc.Id == id, cancellationToken);
        }
    }
}
