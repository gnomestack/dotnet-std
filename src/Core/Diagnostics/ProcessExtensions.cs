using GnomeStack.Functional;

namespace GnomeStack.Diagnostics;

public static class ProcessExtensions
{
#if NETLEGACY
    /// <summary>
    /// Waits asynchronously for the process to exit.
    /// </summary>
    /// <param name="process">The process to wait for cancellation.</param>
    /// <param name="cancellationToken">
    /// A cancellation token. If invoked, the task will return
    /// immediately as canceled.</param>
    /// <returns>A Task representing waiting for the process to end.</returns>
    public static Task WaitForExitAsync(
        this System.Diagnostics.Process process,
        CancellationToken cancellationToken = default)
    {
        if (process.HasExited)
            return Task.CompletedTask;

        var tcs = new TaskCompletionSource<object>();
        process.EnableRaisingEvents = true;

        process.Exited += (sender, args) => tcs.TrySetResult(true);
        if (cancellationToken != default)
            cancellationToken.Register(() => tcs.SetCanceled());

        return process.HasExited ? Task.CompletedTask : tcs.Task;
    }
#endif

    public static ValueResult<PsOutput> ValidateExitCode(this ValueResult<PsOutput> result)
    {
        if (result.IsError)
            return result;

        if (result.Match(o => o.ExitCode != 0))
        {
            var o = result.Unwrap();
            return new ProcessException(o.ExitCode, o.FileName);
        }

        return result;
    }

    public static ValueResult<PsOutput> ValidateExitCode(this ValueResult<PsOutput> result, int validCode)
    {
        if (result.IsError)
            return result;

        if (result.Match(o => o.ExitCode != validCode))
        {
            var o = result.Unwrap();
            return result.And(new ProcessException(o.ExitCode, o.FileName));
        }

        return result;
    }

    public static ValueResult<PsOutput> ValidateExitCode(
        this ValueResult<PsOutput> result,
        int validCode,
        Func<string> createMessage)
    {
        if (result.IsError)
            return result;

        if (result.Match(o => o.ExitCode != validCode))
        {
            var o = result.Unwrap();
            return new ProcessException(o.ExitCode, o.FileName, createMessage());
        }

        return result;
    }
}