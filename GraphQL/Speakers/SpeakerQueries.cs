using GraphQL.Data;
using GraphQL.DataLoader;

namespace GraphQL.Speakers
{
    [QueryType]
    public static class SpeakerQueries
    {
        public static IQueryable<Speaker> GetSpeakers(ApplicationDbContext context) =>
            context.Speakers;

        [NodeResolver]
        public static Task<Speaker> GetSpeaker(int id, SpeakerByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);
    }
}
