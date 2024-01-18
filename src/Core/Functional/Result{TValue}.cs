namespace GnomeStack.Functional;

public class Result<TValue> : Result<TValue, Error>
    where TValue : notnull
{
    public Result(TValue value)
        : base(value)
    {
    }

    public Result(Error error)
        : base(error)
    {
    }

    public Result(Exception exception)
        : base(Functional.Error.Convert(exception))
    {
    }

    public static new Result<TValue> Ok(TValue value)
    {
        return new Result<TValue>(value);
    }

    public static Result<TValue> Error(Exception exception)
    {
        return new Result<TValue>(Functional.Error.Convert(exception));
    }

    public static new Result<TValue> Error(Error error)
    {
        return new Result<TValue>(error);
    }
}