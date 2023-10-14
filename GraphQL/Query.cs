using GraphQL.Data;
using GraphQL.DataLoader;

namespace GraphQL
{
    [QueryType]
    public static class Query
    {
        public static IQueryable<Speaker> GetSpeakers(ApplicationDbContext context) =>
            context.Speakers;

        [NodeResolver]
        public static Task<Speaker> GetSpeaker(int id, SpeakerByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);
    }
}
