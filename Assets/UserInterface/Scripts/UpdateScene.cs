using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class UpdateScene : MonoBehaviour {

    void Start()
    {
        KinectManager.Init();
        ArduinoManager.Init();
        SaveObject.Init();
        SaveObject.LoadTreadmillState();
    }

    void OnApplicationQuit()
    {
        var _KinectApp = Process.GetProcessesByName("KinectFusionExplorer-WPF").First();
        _KinectApp.Kill();
    }
}
