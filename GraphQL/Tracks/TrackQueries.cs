using GraphQL.Data;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Tracks
{
    [QueryType]
    public static class TrackQueries
    {
        [UsePaging]
        public static IQueryable<Track> GetTracks(ApplicationDbContext context) =>
            context.Tracks.OrderBy(t => t.Name);

        public static Task<Track> GetTrackByName(string name, ApplicationDbContext context,
            CancellationToken cancellationToken) =>
            context.Tracks.FirstAsync(t => t.Name == name, cancellationToken);

        public static async Task<IEnumerable<Track>> GetTrackByNames(string[] names, ApplicationDbContext context,
            CancellationToken cancellationToken) =>
            await context.Tracks.Where(t => names.Contains(t.Name)).ToListAsync(cancellationToken);

        [NodeResolver]
        public static Task<Track> GetTrackById(int id, TrackByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public static Task<IReadOnlyList<Track>> GetTracksById(
            [ID(nameof(Track))] int[] ids,
            TrackByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(ids, cancellationToken);
    }
}
