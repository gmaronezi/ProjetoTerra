namespace ProjetoTerra.Shared.Abstractions;

public class FailedResultItem
{
    public string Failure { get; set; }
    public string Cause { get; set; }
    public string Field { get; set; }
}

public class FailedResult
{
    public List<FailedResultItem> Failures { get; set; }

    public FailedResult()
    {
    }

    public FailedResult(List<FailedResultItem> failures)
    {
        Failures = failures;
    }

    public FailedResult(string failure, string cause)
    {
        Failures = new List<FailedResultItem>
        {
            new()
            {
                Failure = failure,
                Cause = cause
            }
        };
    }
    
    public FailedResult(string failure, string cause, string field)
    {
        Failures = new List<FailedResultItem>
        {
            new()
            {
                Failure = failure,
                Cause = cause,
                Field = field
            }
        };
    }
}