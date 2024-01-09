namespace GnomeStack.Functional;

public class Result : Result<Void, Exception>
{
    public Result()
        : base(Void.Value)
    {
    }

    public Result(Exception error)
        : base(error)
    {
    }

    public static Result OkResult { get; } = new Result();

    public static Result<TValue, TError> Ok<TValue, TError>(TValue value, TError error = default!)
        where TValue : notnull
        where TError : notnull
    {
        return new Result<TValue, TError>(value);
    }

    public static Result<TValue> Ok<TValue>(TValue value)
        where TValue : notnull
    {
        return new Result<TValue>(value);
    }

    public static ResultOrError<TValue> OkOrError<TValue>(TValue value)
        where TValue : notnull
    {
        return new ResultOrError<TValue>(value);
    }

    public static ResultOrError<TValue> OkOrError<TValue>(Error error)
        where TValue : notnull
    {
        return new ResultOrError<TValue>(error);
    }

    public static Result<TValue> Error<TValue>(Exception exception)
        where TValue : notnull
    {
        return new Result<TValue>(exception);
    }

    public static Result<TValue, TError> Error<TError, TValue>(TError error)
        where TValue : notnull
        where TError : notnull
    {
        Error<string>(new Exception());
        return new Result<TValue, TError>(error);
    }

    public static Result Try(Action action)
    {
        try
        {
            action();
            return OkResult;
        }
        catch (Exception ex)
        {
            return new Result(ex);
        }
    }

    public static Result<TValue> Try<TValue>(Func<TValue> func)
        where TValue : notnull
    {
        try
        {
            return Ok(func());
        }
        catch (Exception ex)
        {
            return new Result<TValue>(ex);
        }
    }

    public static Result<TValue, TError> Try<TValue, TError>(Func<TValue> action, Func<Exception, TError> error)
        where TValue : notnull
        where TError : notnull
    {
        try
        {
            return new Result<TValue, TError>(action());
        }
        catch (Exception ex)
        {
            return new Result<TValue, TError>(error(ex));
        }
    }

    public static async Task<Result> TryAsync(Func<Task> action)
    {
        try
        {
            await action();
            return OkResult;
        }
        catch (Exception ex)
        {
            return new Result(ex);
        }
    }

    public static async Task<Result> TryAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default)
    {
        try
        {
            await action(cancellationToken)
                .ConfigureAwait(false);

            return OkResult;
        }
        catch (Exception ex)
        {
            return new Result(ex);
        }
    }

    public static async Task<Result<TValue>> TryAsync<TValue>(
        Func<Task<TValue>> action)
        where TValue : notnull
    {
        try
        {
            var value = await action()
                .ConfigureAwait(false);

            return new Result<TValue>(value);
        }
        catch (Exception ex)
        {
            return new Result<TValue>(ex);
        }
    }

    public static async Task<Result<TValue>> TryAsync<TValue>(
        Func<CancellationToken, Task<TValue>> action,
        CancellationToken cancellationToken = default)
        where TValue : notnull
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            var value = await action(cancellationToken)
                .ConfigureAwait(false);

            return new Result<TValue>(value);
        }
        catch (Exception ex)
        {
            return new Result<TValue>(ex);
        }
    }

    public static async Task<Result<TValue, TError>> TryAsync<TValue, TError>(
        Func<Task<TValue>> action,
        Func<Exception, TError> error)
        where TValue : notnull
        where TError : notnull
    {
        try
        {
            var value = await action()
                .ConfigureAwait(false);

            return new Result<TValue, TError>(value);
        }
        catch (Exception ex)
        {
            return new Result<TValue, TError>(error(ex));
        }
    }

    public static async Task<Result<TValue, TError>> TryAsync<TValue, TError>(
        Func<CancellationToken, Task<TValue>> action,
        Func<Exception, TError> error,
        CancellationToken cancellationToken = default)
        where TValue : notnull
        where TError : notnull
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            var value = await action(cancellationToken)
                .ConfigureAwait(false);

            return new Result<TValue, TError>(value);
        }
        catch (Exception ex)
        {
            return new Result<TValue, TError>(error(ex));
        }
    }

    public static ResultOrError TryOrError(Action action)
    {
        try
        {
            action();
            return ResultOrError.OkResult;
        }
        catch (Exception ex)
        {
            return new ResultOrError(ex);
        }
    }

    public static ResultOrError<TValue> TryOrError<TValue>(Func<TValue> func)
        where TValue : notnull
    {
        try
        {
            return OkOrError(func());
        }
        catch (Exception ex)
        {
            return new ResultOrError<TValue>(ex);
        }
    }

    public static async Task<ResultOrError> TryOrErrorAsync(Func<Task> action)
    {
        try
        {
            await action()
                .ConfigureAwait(false);

            return ResultOrError.OkResult;
        }
        catch (Exception ex)
        {
            return new ResultOrError(ex);
        }
    }

    public static async Task<ResultOrError> TryOrErrorAsync(
        Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default)
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            await action(cancellationToken)
                .ConfigureAwait(false);
            return ResultOrError.OkResult;
        }
        catch (Exception ex)
        {
            return new ResultOrError(ex);
        }
    }

    public static async Task<ResultOrError<TValue>> TryOrErrorAsync<TValue>(Func<Task<TValue>> action)
        where TValue : notnull
    {
        try
        {
            var value = await action()
                .ConfigureAwait(false);

            return new ResultOrError<TValue>(value);
        }
        catch (Exception ex)
        {
            return new ResultOrError<TValue>(ex);
        }
    }

    public static async Task<ResultOrError<TValue>> TryOrErrorAsync<TValue>(
        Func<CancellationToken, Task<TValue>> action,
        CancellationToken cancellationToken = default)
        where TValue : notnull
    {
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            var value = await action(cancellationToken)
                .ConfigureAwait(false);

            return new ResultOrError<TValue>(value);
        }
        catch (Exception ex)
        {
            return new ResultOrError<TValue>(ex);
        }
    }
}

public class ResultOrError : ResultOrError<Void>
{
    public ResultOrError()
        : base(Void.Value)
    {
    }

    public ResultOrError(Error error)
        : base(error)
    {
    }

    public static ResultOrError OkResult { get; } = new ResultOrError();

    public static ResultOrError Ok()
    {
        return OkResult;
    }

    public static ResultOrError<TValue> Ok<TValue>(TValue value)
        where TValue : notnull
    {
        return new ResultOrError<TValue>(value);
    }

    public static ResultOrError<TValue> Error<TValue>(Error error)
        where TValue : notnull
    {
        return new ResultOrError<TValue>(error);
    }
}