using GraphQL.Data;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Types
{
    public class AttendeeType : ObjectType<Attendee>
    {
        protected override void Configure(IObjectTypeDescriptor<Attendee> descriptor)
        {
            descriptor
                .Field(d => d.SessionsAttendees)
                .ResolveWith<SessionsAttendeesResolver>(r => r.GetSessions(default!, default!, default!, default))
                .Name("sessions");
        }

        private class SessionsAttendeesResolver
        {
            public async Task<IEnumerable<Session>> GetSessions(
                [Parent] Attendee attendee,
                ApplicationDbContext dbContext,
                SessionByIdDataLoader sessionById,
                CancellationToken cancellationToken)
            {
                var speakerIds = await dbContext.Attendees
                    .Where(a => a.Id == attendee.Id)
                    .Include(a => a.SessionsAttendees)
                    .SelectMany(a => a.SessionsAttendees.Select(t => t.SessionId))
                    .ToArrayAsync(cancellationToken);

                return await sessionById.LoadAsync(speakerIds, cancellationToken);
            }
        }
    }
}
