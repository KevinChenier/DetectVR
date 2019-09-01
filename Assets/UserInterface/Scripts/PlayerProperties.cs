
public class PlayerProperties
{
    public bool HasStartedRunning = false;
    public bool IsRunning = false;
    public bool StartedRunning = false;
    public bool StoppedRunning = false;

    public static PlayerProperties PlayerOne { get; } = new PlayerProperties();
}
