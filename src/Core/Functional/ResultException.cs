namespace GnomeStack.Functional;

public class ResultException : System.Exception
{
    public ResultException()
    {
    }

    public ResultException(string message)
        : base(message)
    {
    }

    public ResultException(string message, System.Exception inner)
        : base(message, inner)
    {
    }

    public static void ThrowIfError(IResult result)
    {
        if (result.IsError)
            throw new OptionException($"{result.GetType().FullName} Failed with an error.");
    }

    public static void ThrowIfError<TValue, TError>(Result<TValue, TError> optional)
        where TValue : notnull
        where TError : notnull
    {
        if (optional.IsError)
        {
            var o = optional.Error();
            if (!o.IsNone)
            {
                var e = o.Unwrap();
                if (e is Exception ex)
                    throw new ResultException($"Result failed with error {ex.Message}", ex);

                throw new ResultException($"Result failed with error {e}.");
            }
            else
            {
                o.Expect($"Result failed with error {o}.");
            }
        }
    }
}