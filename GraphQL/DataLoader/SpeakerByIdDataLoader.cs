using GraphQL.Data;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.DataLoader
{
    public class SpeakerByIdDataLoader : BatchDataLoader<int, Speaker>
    {
        private readonly ApplicationDbContext _dbContext;

        public SpeakerByIdDataLoader(IBatchScheduler batchScheduler, ApplicationDbContext dbContext)
            : base(batchScheduler)
        {
            _dbContext = dbContext;
        }

        protected override async Task<IReadOnlyDictionary<int, Speaker>> LoadBatchAsync(
            IReadOnlyList<int> keys,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Speakers
                .Where(s => keys.Contains(s.Id))
                .ToDictionaryAsync(s => s.Id, cancellationToken);
        }
    }
}
