using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour
{
    public Transform LeftDoor;
    public Transform RightDoor;
    public GameObject Holder;
    public MissionManager MissionMan;

    public void OnPlayClicked()
    {
        StartCoroutine(enterGame());
    }
    IEnumerator enterGame()
    {
        Vector3 leftStart = new Vector3(-250,239, -348);
        Vector3 leftEnd = leftStart + 600*Vector3.left;
        Vector3 rightStart = new Vector3(250, 239, -348);
        Vector3 rightEnd = rightStart + -600 * Vector3.left;

        Holder.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i < 50; i++)
        {
            LeftDoor.position = Vector3.Lerp(leftStart, leftEnd, i / 50f);
            LeftDoor.position = Vector3.Lerp(leftStart, leftEnd, i / 50f);
            RightDoor.position = Vector3.Lerp(rightStart, rightEnd, i / 50f);
            RightDoor.position = Vector3.Lerp(rightStart, rightEnd, i / 50f);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        MissionMan.SetupMission();
    }
}
