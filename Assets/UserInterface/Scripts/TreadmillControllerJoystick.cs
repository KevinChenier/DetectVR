using UnityEngine;

public class TreadmillControllerJoystick : MonoBehaviour
{
    public GameObject PlayerController;

    void Update()
    {
        float y = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;

        if (y == 0)
        {
            PlayerProperties.PlayerOne.HasStartedRunning = false;
            PlayerProperties.PlayerOne.IsRunning = false;
            ArduinoManager.Board.analogWrite((int)ArduinoManager.Pin.Speed, 4);
            PlayerController.GetComponent<OVRPlayerController>().Acceleration = 0.1f;
            //Debug.Log("ZERO: " + OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick));
        }
        if (y < 0.5 && y > 0)
        {
            PlayerProperties.PlayerOne.HasStartedRunning = true;
            PlayerProperties.PlayerOne.IsRunning = true;
            ArduinoManager.Board.analogWrite((int)ArduinoManager.Pin.Speed, 20);
            PlayerController.GetComponent<OVRPlayerController>().Acceleration = 0.13f;
            //Debug.Log("WALK: " + OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick));
        }
        if (y < 1 && y > 0.5)
        {
            PlayerProperties.PlayerOne.IsRunning = true;
            ArduinoManager.Board.analogWrite((int)ArduinoManager.Pin.Speed, 50);
            PlayerController.GetComponent<OVRPlayerController>().Acceleration = 0.15f;
            //Debug.Log("JOG: " + OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick));
        }
        if (y > 0.8 && OVRInput.Get(OVRInput.Button.PrimaryThumbstick))
        {
            PlayerProperties.PlayerOne.HasStartedRunning = true;
            PlayerProperties.PlayerOne.IsRunning = true;
            ArduinoManager.Board.analogWrite((int)ArduinoManager.Pin.Speed, 80);
            PlayerController.GetComponent<OVRPlayerController>().Acceleration = 0.2f;
            //Debug.Log("SPRINT: " + OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick));
        }

        //TO CHANGE, THERE ARE CHANCES THAT THE UPDATE MISSES THE CONDITION...
        if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) != Vector2.zero && !PlayerProperties.PlayerOne.HasStartedRunning)
                SaveObject.TreadmillLastPositionBeforeRun = GameObject.Find("Treadmill").transform.position;

        SaveObject.TryToModifyTreadmillConfiguration();
        SaveObject.TryToOverwriteTreadmillConfigurationFile();
    }
}
