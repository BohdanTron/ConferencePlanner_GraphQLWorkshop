using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Sessions
{
    [MutationType]
    public class SessionMutations
    {
        public async Task<MutationResult<Session, UserError>> AddSession(
            AddSessionInput input,
            ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(input.Title))
            {
                return new UserError("The title cannot be empty.", "TITLE_EMPTY");
            }

            if (input.SpeakerIds.Count == 0)
            {
                return new UserError("No speaker assigned.", "NO_SPEAKER");
            }

            var session = new Session
            {
                Title = input.Title,
                Abstract = input.Abstract,
            };

            foreach (var speakerId in input.SpeakerIds)
            {
                session.SessionSpeakers.Add(new SessionSpeaker
                {
                    SpeakerId = speakerId
                });
            }

            context.Sessions.Add(session);
            await context.SaveChangesAsync(cancellationToken);

            return session;
        }

        public async Task<MutationResult<Session, UserError>> ScheduleSession(
            ScheduleSessionInput input,
            ApplicationDbContext context,
            CancellationToken cancellationToken)
        {
            if (input.EndTime < input.StartTime)
            {
                return new UserError("endTime has to be larger than startTime.", "END_TIME_INVALID");
            }

            var session = await context.Sessions.FindAsync(input.SessionId);

            if (session is null)
            {
                return new UserError("Session not found.", "SESSION_NOT_FOUND");
            }

            session.TrackId = input.TrackId;
            session.StartTime = input.StartTime;
            session.EndTime = input.EndTime;

            await context.SaveChangesAsync(cancellationToken);

            return session;
        }

        public record AddSessionInput(
            string Title,
            string? Abstract,
            [property: ID(nameof(Speaker))] IReadOnlyList<int> SpeakerIds);

        public record ScheduleSessionInput(
            [property: ID(nameof(Session))] int SessionId,
            [property: ID(nameof(Track))] int TrackId,
            DateTimeOffset StartTime,
            DateTimeOffset EndTime);
    }
}
