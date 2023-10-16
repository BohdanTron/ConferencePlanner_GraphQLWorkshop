using GraphQL.Data;
using GraphQL.DataLoader;

namespace GraphQL.Sessions
{
    [QueryType]
    public static class SessionQueries
    {
        public static IQueryable<Session> GetSessions(ApplicationDbContext context) =>
            context.Sessions;

        [NodeResolver]
        public static Task<Session> GetSessionById(int id, SessionByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(id, cancellationToken);

        public static Task<IReadOnlyList<Session>> GetSessionsById(
            [ID(nameof(Session))] int[] ids,
            SessionByIdDataLoader dataLoader,
            CancellationToken cancellationToken) =>
            dataLoader.LoadAsync(ids, cancellationToken);
    }
}
