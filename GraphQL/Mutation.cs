using GraphQL.Data;

namespace GraphQL
{
    [MutationType]
    public class Mutation
    {
        public async Task<AddSpeakerPayload> AddSpeaker(
            AddSpeakerInput input,
            [Service] ApplicationDbContext context)
        {
            var speaker = new Speaker
            {
                Name = input.Name,
                Bio = input.Bio,
                WebSite = input.Website
            };

            context.Speakers.Add(speaker);
            await context.SaveChangesAsync();

            return new AddSpeakerPayload(speaker);
        }

    }
}
