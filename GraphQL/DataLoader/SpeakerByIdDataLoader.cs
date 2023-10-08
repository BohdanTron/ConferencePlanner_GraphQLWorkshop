using GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.DataLoader
{
    public class SpeakerByIdDataLoader : BatchDataLoader<int, Speaker>
    {
        private readonly IServiceProvider _serviceProvider;

        public SpeakerByIdDataLoader(IBatchScheduler batchScheduler, IServiceProvider serviceProvider)
            : base(batchScheduler)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task<IReadOnlyDictionary<int, Speaker>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return await dbContext.Speakers
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(s => s.Id, cancellationToken);
        }
    }
}
