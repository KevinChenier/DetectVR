using MemoryMappedFileManager;
using System;
using System.Diagnostics;
using System.Threading;
using static GlobalConstants;


public class KinectManager
{
    public static void Init()
    {
        ProcessStartInfo _ProcessInfo = new ProcessStartInfo();
        _ProcessInfo.FileName = KINECT_FUSION_PATH;
        _ProcessInfo.WindowStyle = ProcessWindowStyle.Minimized;
        Process.Start(_ProcessInfo);

        InitCommunicationKinectUnity();
    }

    private static void InitCommunicationKinectUnity()
    {
        MemoryMappedFileCommunicator _Communicator = new MemoryMappedFileCommunicator("MemoryMappedShare", 10000000)
        {
            WritePosition = 0,
            ReadPosition = 0
        };
        _Communicator.DataReceived += new EventHandler<MemoryMappedDataReceivedEventArgs>(_Communicator_DataReceived);
        _Communicator.StartReader();
    }

    private static void _Communicator_DataReceived(object sender, MemoryMappedDataReceivedEventArgs e)
    {
            UnityEngine.Debug.Log("Data received");
            new Thread(() => UpdateLegs.CalculateNewMesh(System.Text.Encoding.ASCII.GetString(e.Data).Split('@'))).Start();
    }
}