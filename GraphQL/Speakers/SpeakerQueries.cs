using GraphQL.Data;
using GraphQL.DataLoader;

namespace GraphQL.Speakers
{
    [QueryType]
    public static class SpeakerQueries
    {
        [UsePaging]
        public static IQueryable<Speaker> GetSpeakers(ApplicationDbContext context) =>
            context.Speakers;

        [NodeResolver]
        public static Task<Speaker> GetSpeakerById(int id, SpeakerByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public static Task<IReadOnlyList<Speaker>> GetSpeakersById(
            [ID(nameof(Speaker))] int[] ids,
            SpeakerByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(ids, cancellationToken);
    }
}
