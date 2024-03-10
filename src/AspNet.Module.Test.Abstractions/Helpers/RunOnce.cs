namespace AspNet.Module.Test.Helpers;

public class RunOnce
{
    private bool _initialized;

    public void Execute(Action action)
    {
        if (_initialized) return;
        lock (this)
        {
            if (_initialized) return;
            action();
            _initialized = true;
        }
    }
}