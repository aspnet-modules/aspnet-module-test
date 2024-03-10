namespace AspNet.Module.Test.Helpers;

public class AsyncRunOnce
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private bool _initialized;

    public async Task Execute(Func<Task> action)
    {
        if (_initialized) return;
        try
        {
            await _semaphore.WaitAsync();
            if (_initialized) return;
            await action();
            _initialized = true;
        }
        finally
        {
            _semaphore.Release();
        }
    }
}