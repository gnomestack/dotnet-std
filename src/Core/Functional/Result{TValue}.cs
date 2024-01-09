namespace GnomeStack.Functional;

public class Result<TValue> : Result<TValue, Exception>
    where TValue : notnull
{
    public Result(TValue value)
        : base(value)
    {
    }

    public Result(Exception error)
        : base(error)
    {
    }

    public static new Result<TValue> Ok(TValue value)
    {
        return new Result<TValue>(value);
    }

    public static new Result<TValue> Error(Exception exception)
    {
        return new Result<TValue>(exception);
    }
}

public class ResultOrError<TValue> : Result<TValue, Error>
    where TValue : notnull
{
    public ResultOrError(TValue value)
        : base(value)
    {
    }

    public ResultOrError(Error error)
        : base(error)
    {
    }

    public static new ResultOrError<TValue> Ok(TValue value)
    {
        return new ResultOrError<TValue>(value);
    }

    public static new ResultOrError<TValue> Error(Error error)
    {
        return new ResultOrError<TValue>(error);
    }
}