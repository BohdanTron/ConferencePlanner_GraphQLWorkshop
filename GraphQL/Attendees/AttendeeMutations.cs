using GraphQL.Common;
using GraphQL.Data;
using GraphQL.DataLoader;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.Attendees
{
    [MutationType]
    public class AttendeeMutations
    {
        public async Task<MutationResult<Attendee>> RegisterAttendeeAsync(
            RegisterAttendeeInput input,
            ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            var attendee = new Attendee
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                UserName = input.UserName,
                EmailAddress = input.EmailAddress
            };

            context.Attendees.Add(attendee);

            await context.SaveChangesAsync(cancellationToken);

            return attendee;
        }

        public async Task<MutationResult<CheckInAttendeePayload, UserError>> CheckInAttendee(
            CheckInAttendeeInput input,
            ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            var attendee = await context.Attendees
                .FirstOrDefaultAsync(t => t.Id == input.AttendeeId, cancellationToken);

            if (attendee is null)
            {
                return new UserError("Attendee not found.", "ATTENDEE_NOT_FOUND");
            }

            attendee.SessionsAttendees.Add(
                new SessionAttendee
                {
                    SessionId = input.SessionId
                });

            await context.SaveChangesAsync(cancellationToken);

            return new CheckInAttendeePayload(attendee, input.SessionId);
        }
    }

    public record RegisterAttendeeInput(
        string FirstName,
        string LastName,
        string UserName,
        string EmailAddress);

    public record CheckInAttendeeInput(
        [property: ID(nameof(Session))] int SessionId,
        [property: ID(nameof(Attendee))] int AttendeeId);

    public class CheckInAttendeePayload
    {
        private readonly int _sessionId;

        public Attendee Attendee { get; }

        public CheckInAttendeePayload(Attendee attendee, int sessionId)
        {
            Attendee = attendee;
            _sessionId = sessionId;
        }

        public async Task<Session?> GetSession(
            SessionByIdDataLoader sessionById,
            CancellationToken cancellationToken)
        {
            return await sessionById.LoadAsync(_sessionId, cancellationToken);
        }

    }
}
