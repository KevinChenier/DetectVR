using System.IO;

public class GlobalConstants
{
    public const string HOST = "127.0.0.1";
    public const int PORT = 9988;
    public const int TREADMILL_MIN_VALUE = 4;
    public static readonly string SAVE_FOLDER = Directory.GetCurrentDirectory() + "/Saves/";
    public static readonly string TREADMILL_CONFIGURATION_PATH = Directory.GetCurrentDirectory() + "/Saves/TreadmillConfiguration.json";
    public static readonly string KINECT_FUSION_PATH = Directory.GetCurrentDirectory() + "/Kinect/C#/KinectFusionExplorer-WPF/bin/Debug/KinectFusionExplorer-WPF.exe";
}
