namespace GraphQL.Common
{
    public abstract class Payload
    {
        public IReadOnlyList<UserError>? Errors { get; }

        protected Payload(IReadOnlyList<UserError>? errors = null) =>
            Errors = errors;
    }

    public record UserError(string Message, string Code);
}
