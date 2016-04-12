using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class Manual : MonoBehaviour
{
    public GameObject GUI;
    public Text Page1;
    public Text Page2;
    public GameObject NextButton;
    public GameObject BackButton;
    public string[] ManualPaths;

    Transform trans;
    AudioSource audioSource;
    Animation anim;
    BoxCollider coll;

    List<string> pages;
    int currentPage;

    bool focused;
    Vector3 startPosition;
    Quaternion startRotation;
    Vector3 focusPosition;
    Quaternion focusRotation;
    Vector3 colliderCenter;
    Vector3 colliderSize;

    void Start()
    {
        trans = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animation>();
        coll = GetComponent<BoxCollider>();
        colliderCenter = coll.center;
        colliderSize = coll.size;

        startPosition = trans.position;
        startRotation = trans.rotation;
        Transform focusTarget = GameObject.Find("ManualFocus").GetComponent<Transform>();
        focusPosition = focusTarget.position;
        focusRotation = focusTarget.rotation;
        if (ManualPaths.Length>0)
        {
            Populate(ManualPaths);
        }
    }

    public void Populate(string[] manualPaths)
    {
        Page1.text = "";
        Page2.text = "";

        pages = new List<string>();
        foreach (string path in manualPaths)
        {
            DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/StreamingAssets/Manuals/" + path);
            foreach (FileInfo file in di.GetFiles("*.txt"))
            {
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    pages.Add(sr.ReadToEnd());
                }
            }
        }

        currentPage = 0;
        Page1.text = pages[currentPage];
        if (currentPage + 1 < pages.Count)
        {
            Page2.text = pages[currentPage + 1];
        }
        if (currentPage < pages.Count - 2)
        {
            NextButton.SetActive(true);
        }
        else
        {
            NextButton.SetActive(false);
        }
        if (currentPage > 0)
        {
            BackButton.SetActive(true);
        }
        else
        {
            BackButton.SetActive(false);
        }
    }
    public void OnNextClicked()
    {
        Page1.text = "";
        Page2.text = "";
        audioSource.Play();
        currentPage += 2;
        Page1.text = pages[currentPage];
        if (currentPage + 1 < pages.Count)
        {
            Page2.text = pages[currentPage + 1];
        }
        if (currentPage >= pages.Count - 1)
        {
            NextButton.SetActive(false);
        }
        if (currentPage > 0)
        {
            BackButton.SetActive(true);
        }
    }
    public void OnBackClicked()
    {
        Page1.text = "";
        Page2.text = "";
        audioSource.Play();
        currentPage -= 2;
        Page1.text = pages[currentPage];
        if (currentPage + 1 < pages.Count)
        {
            Page2.text = pages[currentPage + 1];
        }
        if (currentPage < pages.Count - 1)
        {
            NextButton.SetActive(true);
        }
        if (currentPage == 0)
        {
            BackButton.SetActive(false);
        }
    }
    void OnMouseUpAsButton()
    {
        if (focused)
        {
            anim.Play("Close");
            GUI.SetActive(false);
            coll.center = colliderCenter;
            coll.size = colliderSize;
            StartCoroutine(MoveManual(startPosition, startRotation));
        }
        else
        {
            anim.Play("Open");
            StartCoroutine(MoveManual(focusPosition, focusRotation));
            StartCoroutine(ShowGUI());
        }
    }
    IEnumerator MoveManual(Vector3 target, Quaternion rotation)
    {
        Vector3 sPosition = trans.position;
        Quaternion sRotation = trans.rotation;
        for (int i = 0; i < 50; i++)
        {
            trans.position = Vector3.Lerp(sPosition, target, i / 50f);
            trans.rotation = Quaternion.Lerp(sRotation, rotation, i / 50f);
            yield return null;
        }
        focused = !focused;
    }
    IEnumerator ShowGUI()
    {
        while (anim.isPlaying)
        {
            yield return null;
        }
        GUI.SetActive(true);

        coll.center = new Vector3(colliderCenter.x+colliderSize.x/2, colliderCenter.y-colliderSize.y/2, colliderCenter.z);
        coll.size = new Vector3(colliderSize.x*2,colliderSize.y/2,colliderSize.z);
    }
}
