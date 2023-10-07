using GraphQL.Data;

namespace GraphQL
{
    public class Query
    {
        public IQueryable<Speaker> GetSpeakers([Service] ApplicationDbContext context) =>
            context.Speakers;
    }
}
