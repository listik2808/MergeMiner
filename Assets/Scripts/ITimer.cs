using System;

public interface ITimer
{
    public abstract event Action Started;
    public abstract event Action Stopped;
    public abstract event Action Completed;
    public abstract event Action Updated;

    public abstract float TotalTime { get; }
    public abstract float TimeLeft { get; }
}