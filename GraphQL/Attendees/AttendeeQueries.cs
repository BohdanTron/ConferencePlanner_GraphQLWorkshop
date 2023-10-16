using GraphQL.Data;
using GraphQL.DataLoader;

namespace GraphQL.Attendees
{
    [QueryType]
    public class AttendeeQueries
    {
        [UsePaging]
        public IQueryable<Attendee> GetAttendees(ApplicationDbContext context) =>
            context.Attendees;

        [NodeResolver]
        public Task<Attendee> GetAttendeeById(
            int id,
            AttendeeByIdDataLoader attendeeById,
            CancellationToken cancellationToken) =>
            attendeeById.LoadAsync(id, cancellationToken);

        public async Task<IEnumerable<Attendee>> GetAttendeesById(
            [ID(nameof(Attendee))] int[] ids,
            AttendeeByIdDataLoader attendeeById,
            CancellationToken cancellationToken) =>
            await attendeeById.LoadAsync(ids, cancellationToken);
    }
}
