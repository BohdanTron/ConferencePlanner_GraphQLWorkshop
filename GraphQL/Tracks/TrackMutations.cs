using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Tracks
{
    [MutationType]
    public class TrackMutations
    {
        public async Task<Track> AddTrack(
            AddTrackInput input,
            ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            var track = new Track { Name = input.Name };
            context.Tracks.Add(track);

            await context.SaveChangesAsync(cancellationToken);

            return track;
        }

        public async Task<MutationResult<Track, UserError>> RenameTrack(
            RenameTrackInput input,
            ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            var track = await context.Tracks.FindAsync(input.Id);
            if (track is null)
            {
                return new UserError("Track not found", "INVALID_TRACK_ID");
            }

            track.Name = input.Name;

            await context.SaveChangesAsync(cancellationToken);

            return track;
        }
    }

    public record AddTrackInput(string Name);

    public record RenameTrackInput([property: ID(nameof(Track))] int Id, string Name);
}
