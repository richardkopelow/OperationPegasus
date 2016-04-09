using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugHUD : MonoBehaviour
{
    public InputField MissionNumberField;

    public void OnClearDataClick()
    {
        PlayerPrefs.DeleteAll();
    }
    public void OnSetMissionClick()
    {
        PlayerPrefs.SetInt("CurrentMission", int.Parse(MissionNumberField.text));
    }
}
