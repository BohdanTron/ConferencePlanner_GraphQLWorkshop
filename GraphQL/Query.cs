using GraphQL.Data;

namespace GraphQL
{
    public class Query
    {
        public IQueryable<Speaker> GetSpeakers(ApplicationDbContext context) =>
            context.Speakers;
    }
}
