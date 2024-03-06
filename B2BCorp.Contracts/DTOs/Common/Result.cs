namespace B2BCorp.Contracts.DTOs.Common
{
    public class Result<T>(T value) : Result
    {
        public T Value { get; set; } = value;
    }

    public class Result()
    {
        public Result(Outcomes outcome, string property, object value, string error) : this()
        {
            Outcome = outcome;
            PropertyError.Add($"{property} = '{value}'", error);
        }

        public bool Successful => Outcome == Outcomes.Success;
        public bool ConcurrencyError => Outcome == Outcomes.ConcurrencyError;

        public Outcomes Outcome { get; set; } = Outcomes.Success;
        public Dictionary<string, string> PropertyError { get; set; } = [];
    }

    public enum Outcomes
    {
        Success,
        ValidationError,
        ConcurrencyError,
        GeneralError
    }
}
