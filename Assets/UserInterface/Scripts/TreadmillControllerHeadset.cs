using System;
using UnityEngine;

public class TreadmillControllerHeadset : MonoBehaviour
{
    private const float HARDCODED_THRESHOLD = 0.02f;

    void Update()
    {
        if (OVRPlugin.GetNodePose(nodeId: OVRPlugin.Node.EyeCenter, stepId: OVRPlugin.Step.Physics).Position.x < HARDCODED_THRESHOLD)
        {
            PlayerProperties.PlayerOne.HasStartedRunning = true;
            PlayerProperties.PlayerOne.IsRunning = true;

            ArduinoManager.Board.analogWrite((int)ArduinoManager.Pin.Speed, 22);
            BotsimuDataSender.SendUDP(BitConverter.GetBytes(true));
        }

        if (OVRPlugin.GetNodePose(nodeId: OVRPlugin.Node.EyeCenter, stepId: OVRPlugin.Step.Physics).Position.x >= HARDCODED_THRESHOLD)
        {
            PlayerProperties.PlayerOne.IsRunning = false;

            ArduinoManager.Board.analogWrite((int)ArduinoManager.Pin.Speed, 4);
            BotsimuDataSender.SendUDP(BitConverter.GetBytes(false));
        }

        //TO CHANGE, THERE ARE CHANCES THAT THE UPDATE MISSES THE CONDITION...
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) != Vector2.zero && !PlayerProperties.PlayerOne.HasStartedRunning)
            SaveObject.TreadmillLastPositionBeforeRun = GameObject.Find("Treadmill").transform.position;

        SaveObject.TryToModifyTreadmillConfiguration();
        SaveObject.TryToOverwriteTreadmillConfigurationFile();
    }
}
