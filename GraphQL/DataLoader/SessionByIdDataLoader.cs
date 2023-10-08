using GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.DataLoader
{
    public class SessionByIdDataLoader : BatchDataLoader<int, Session>
    {
        private readonly IServiceProvider _serviceProvider;

        public SessionByIdDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider)
            : base(batchScheduler)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<IReadOnlyDictionary<int, Session>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await dbContext.Sessions
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}
