using Uduino;
using static GlobalConstants;

public class ArduinoManager
{
    public enum Pin : int
    {
        Speed = 3,
    }

    public static bool PinsWereReset = false;
    public static UduinoManager Board = UduinoManager.Instance;

    public static void Init()
    {
        Board.pinMode((int)Pin.Speed, PinMode.PWM);
        Board.analogWrite((int)Pin.Speed, TREADMILL_MIN_VALUE);
    }
}
