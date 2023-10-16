using GraphQL.Common;
using GraphQL.Data;

namespace GraphQL.Speakers
{
    [MutationType]
    public class SpeakerMutations
    {
        public async Task<MutationResult<Speaker, UserError>> AddSpeaker(
            AddSpeakerInput input,
            ApplicationDbContext context)
        {
            if (string.IsNullOrEmpty(input.Name))
            {
                return new UserError("The name cannot be empty", "NAME_EMPTY");
            }

            var speaker = new Speaker
            {
                Name = input.Name,
                Bio = input.Bio,
                WebSite = input.Website
            };

            context.Speakers.Add(speaker);
            await context.SaveChangesAsync();

            return speaker;
        }

        public async Task<MutationResult<Speaker, UserError>> DeleteSpeaker(
            [ID(nameof(Speaker))] int id,
            ApplicationDbContext context)
        {
            var speaker = await context.Speakers.FindAsync(id);
            if (speaker is null)
            {
                return new UserError("Speaker not found", "INVALID_SPEAKER_ID");
            }

            context.Remove(speaker);
            await context.SaveChangesAsync();

            return speaker;
        }
    }

    public record AddSpeakerInput(
        string Name,
        string? Bio,
        string? Website);
}
