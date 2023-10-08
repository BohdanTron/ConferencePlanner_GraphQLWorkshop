using GraphQL.Data;
using GraphQL.DataLoader;

namespace GraphQL
{
    public class Query
    {
        public IQueryable<Speaker> GetSpeakers(ApplicationDbContext context) =>
            context.Speakers;

        public Task<Speaker> GetSpeaker(int id, SpeakerByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);
    }
}
