using System.IO;
using UnityEngine;

using static GlobalConstants;

public class SaveObject
{
    public static Vector3 TreadmillLastPositionBeforeRun;

    public Vector3 TreadmillScaleAxis;
    public Vector3 TreadmillWorldPosition;
    public Vector3 TreadmillWorldRotation;

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
            Directory.CreateDirectory(SAVE_FOLDER);
    }

    public static void LoadTreadmillState()
    {
        if (File.Exists(TREADMILL_CONFIGURATION_PATH))
        {
            string _SavedString = File.ReadAllText(TREADMILL_CONFIGURATION_PATH);

            if (_SavedString != string.Empty)
            {
                SaveObject _SavedObject = JsonUtility.FromJson<SaveObject>(_SavedString);

                GameObject.Find("Treadmill").transform.localScale = _SavedObject.TreadmillScaleAxis;
                GameObject.Find("Treadmill").transform.position = _SavedObject.TreadmillWorldPosition;
                GameObject.Find("Treadmill").transform.eulerAngles = _SavedObject.TreadmillWorldRotation;
            }
        }
        else 
            File.Create(TREADMILL_CONFIGURATION_PATH);
    }

    public static void TryToModifyTreadmillConfiguration()
    {
        if (OVRInput.Get(OVRInput.Button.One))
            ChangeTreadmillPositionAndRotation();

        if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            ScaleTreadmillAxisZ();

        if (OVRInput.Get(OVRInput.Button.SecondaryIndexTrigger))
            ScaleTreadmillAxisX();
    }

    public static void TryToOverwriteTreadmillConfigurationFile()
    {
        if (OVRInput.GetUp(OVRInput.Button.One) || OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger) || OVRInput.GetUp(OVRInput.Button.SecondaryIndexTrigger))
            SaveTreadmillState();
    }

    private static void SaveTreadmillState()
    {
        SaveObject _SaveObject;

        if (!PlayerProperties.PlayerOne.HasStartedRunning)
        {
            _SaveObject = new SaveObject
            {
                TreadmillScaleAxis = GameObject.Find("Treadmill").transform.localScale,
                TreadmillWorldPosition = GameObject.Find("Treadmill").transform.position,
                TreadmillWorldRotation = GameObject.Find("Treadmill").transform.eulerAngles
            };
        }
        else
        {
            _SaveObject = new SaveObject
            {
                TreadmillScaleAxis = GameObject.Find("Treadmill").transform.localScale,
                TreadmillWorldPosition = TreadmillLastPositionBeforeRun,
                TreadmillWorldRotation = GameObject.Find("Treadmill").transform.eulerAngles
            };
        }
        File.WriteAllText(TREADMILL_CONFIGURATION_PATH, JsonUtility.ToJson(_SaveObject));
    }

    private static void ChangeTreadmillPositionAndRotation()
    {
        GameObject.Find("Treadmill").transform.parent = GameObject.Find("hand_right").transform;
        GameObject.Find("Treadmill").transform.localPosition = new Vector3(-0.2f, 0f, -0.2f);
        GameObject.Find("Treadmill").transform.parent = GameObject.Find("LocalAvatar").transform;
        GameObject.Find("Treadmill").transform.position -= new Vector3(0f, 1f, 0f);
        GameObject.Find("Treadmill").transform.eulerAngles = new Vector3(0f, GameObject.Find("hand_right").transform.eulerAngles.y - 90f, 0f);
    }

    private static void ScaleTreadmillAxisX()
    {
        GameObject.Find("Treadmill").transform.localScale = new Vector3(Vector3.Distance(GameObject.Find("hand_left").transform.position, GameObject.Find("hand_right").transform.position) / 3, GameObject.Find("Treadmill").transform.localScale.y, GameObject.Find("Treadmill").transform.localScale.z);
    }

    private static void ScaleTreadmillAxisZ()
    {
        GameObject.Find("Treadmill").transform.localScale = new Vector3(GameObject.Find("Treadmill").transform.localScale.x, GameObject.Find("Treadmill").transform.localScale.y, Vector3.Distance(GameObject.Find("hand_left").transform.position, GameObject.Find("hand_right").transform.position) / 3);
    }
}
