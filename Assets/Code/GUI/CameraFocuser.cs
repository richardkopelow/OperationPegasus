using UnityEngine;
using System.Collections;

public class CameraFocuser : MonoBehaviour
{
    public Transform Home;
    public Transform Target;
    public Transform Camera;

    public bool AtTarget;
    public bool Toggle = true;

    public void SetAtTarget(bool val)
    {
        AtTarget = val;
    }
    public void OnClick()
    {
        if (AtTarget)
        {
            StartCoroutine(MoveCamera(Home.position));
        }
        else
        {
            StartCoroutine(MoveCamera(Target.position));
        }
    }
    IEnumerator MoveCamera(Vector3 target)
    {
        Vector3 startPosition = Camera.position;
        for (int i = 0; i < 50; i++)
        {
            Camera.position = Vector3.Lerp(startPosition, target, i / 50f);
            yield return null;
        }
        if (Toggle)
        {
            AtTarget = !AtTarget;
        }
    }
}
