using UnityEngine;
using System.Collections;

public class DebugHUD : MonoBehaviour
{

    public void OnClearDataClick()
    {
        PlayerPrefs.DeleteAll();
    }
}
