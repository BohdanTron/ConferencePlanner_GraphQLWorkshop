using GraphQL.Data;

namespace GraphQL
{
    public class AddSpeakerPayload
    {
        public Speaker Speaker { get; }

        public AddSpeakerPayload(Speaker speaker) =>
            Speaker = speaker;
    }
}
