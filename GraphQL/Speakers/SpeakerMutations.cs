using GraphQL.Data;

namespace GraphQL.Speakers
{
    [MutationType]
    public class SpeakerMutations
    {
        public async Task<AddSpeakerPayload> AddSpeaker(
            AddSpeakerInput input,
            ApplicationDbContext context)
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
