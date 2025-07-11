namespace MVC.Shared
{
    public sealed record Error(int Code, string Description)
    {
        public static readonly Error None = new(400, string.Empty);
        public static readonly Error NullValue = new(400, "Unkown Error");

        public static implicit operator Result(Error error) => Result.Failure(error);

        public Result ToResult() => Result.Failure(this);
    }
}
