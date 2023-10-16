using GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.DataLoader
{
    public class TrackByIdDataLoader : BatchDataLoader<int, Track>
    {
        private readonly IServiceProvider _serviceProvider;

        public TrackByIdDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider)
            : base(batchScheduler)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<IReadOnlyDictionary<int, Track>> LoadBatchAsync(IReadOnlyList<int> keys, CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await dbContext.Tracks
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(t => t.Id, cancellationToken);
        }
    }
}
