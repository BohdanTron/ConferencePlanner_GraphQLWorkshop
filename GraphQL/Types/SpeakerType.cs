using GraphQL.Data;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types
{
    public class SpeakerType : ObjectType<Speaker>
    {
        protected override void Configure(IObjectTypeDescriptor<Speaker> descriptor)
        {
            descriptor
                .Field(d => d.SessionSpeakers)
                .ResolveWith<SessionSpeakersResolver>(r => r.GetSessions(default!, default!, default!, default))
                .Name("sessions");
        }

        private class SessionSpeakersResolver
        {
            public async Task<IEnumerable<Session>> GetSessions(
                [Parent] Speaker speaker,
                ApplicationDbContext dbContext,
                SessionByIdDataLoader sessionById,
                CancellationToken cancellationToken)
            {
                var sessionIds = await dbContext.Speakers
                    .Where(s => s.Id == speaker.Id)
                    .Include(s => s.SessionSpeakers)
                    .SelectMany(s => s.SessionSpeakers.Select(t => t.SessionId))
                    .ToArrayAsync(cancellationToken);

                return await sessionById.LoadAsync(sessionIds, cancellationToken);
            }
        }
    }
}
