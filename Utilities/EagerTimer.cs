using System;
using System.ComponentModel;
using System.Timers;

namespace Utilities
{
/// <summary>
// EagerTimer is a simple wrapper around System.Timers.Timer that
// provides "set up and immediately execute" functionality by adding a
// new AutoStart property, and also provides the ability to manually
// raise the Elapsed event with RaiseElapsed.
/// </summary>
public class EagerTimer : Timer
{
    public EagerTimer(): base() { }

    public EagerTimer(double interval) : base(interval) { }

    // Need to hide this so we can use Elapsed.Invoke below
    // (otherwise the compiler complains)
    private event ElapsedEventHandler ElapsedHandler;
    public new event ElapsedEventHandler Elapsed
    {
        add { ElapsedHandler += value; base.Elapsed += value; }
        remove { ElapsedHandler -= value; base.Elapsed -= value; }
    }

    public new void Start()
    {
        // If AutoStart is enabled, we need to invoke the timer event manually
        if (AutoStart)
        {
            var onElapsedHandler = ElapsedHandler;
            if (onElapsedHandler != null)
                onElapsedHandler.BeginInvoke(this, null, AutoStartCallback, ElapsedHandler); // fire immediately
        }

        // Proceed as normal
        base.Start();
    }

    public void Reset()
    {
        Stop();
        Start();        
    }


    private void AutoStartCallback(IAsyncResult result)
    {
        
        try
        {
                var handler = result.AsyncState as ElapsedEventHandler;
                if (handler != null) handler.EndInvoke(result);
        }
        catch { }
    }

    // Summary:
    //     Gets or sets a value indicating whether the EagerTimer should raise
    //     the System.Timers.Timer.Elapsed event immediately when Start() is called,
    //     or only after the first time it elapses. If AutoStart is false, EagerTimer behaves
    //     identically to System.Timers.Timer.
    //
    // Returns:
    //     true if the EagerTimer should raise the System.Timers.Timer.Elapsed
    //     event immediately when Start() is called; false if it should raise the System.Timers.Timer.Elapsed
    //     event only after the first time the interval elapses. The default is true.
    [Category("Behavior")]
    [DefaultValue(true)]
    [TimersDescription("TimerAutoStart")]
    public bool AutoStart { get; set; }

    /// <summary>
    /// Manually raises the Elapsed event of the System.Timers.Timer.
    /// </summary>
    public void RaiseElapsed()
    {
        if (ElapsedHandler != null)
            ElapsedHandler(this, null);
    }
}
}

