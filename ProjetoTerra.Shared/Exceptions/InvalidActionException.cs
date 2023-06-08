using ProjetoTerra.Shared.Abstractions;

namespace ProjetoTerra.Shared.Exceptions;

public class InvalidActionException : Exception
{
    public string Failure { get; set; }
    public string Cause { get; set; }
    public string Field { get; set; }

    public InvalidActionException(string failure, string field)
    {
        Failure = failure;
        Field = field;
    }
    public InvalidActionException(string failure, string cause, string field)
    {
        Failure = failure;
        Cause = cause;
        Field = field;
    }

    public InvalidActionException(string failure)
    {
        Failure = failure;
    }

    public FailedResult AsFailedResult()
    {
        return new FailedResult(
            Failure,
            Cause,
            Field
        );
    }
}