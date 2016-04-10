using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Collections;

public class Manual : MonoBehaviour
{
    public Text Page1;
    public Text Page2;
    public GameObject NextButton;
    public GameObject BackButton;
    public string ManualPath="";

    Transform trans;
    AudioSource audioSource;

    List<string> pages;
    int currentPage;

    bool focused;
    Vector3 startPosition;
    Vector3 focusPosition;

    void Start()
    {
        trans = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        startPosition = trans.position;
        focusPosition = GameObject.Find("ManualFocus").GetComponent<Transform>().position;
        if (ManualPath!="")
        {
            Populate(ManualPath);
        }
    }

    public void Populate(string manualPath)
    {
        Page1.text = "";
        Page2.text = "";

        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/StreamingAssets/Manuals/" + manualPath);
        pages = new List<string>();
        foreach (FileInfo file in di.GetFiles("*.txt"))
        {
            using (StreamReader sr=new StreamReader(file.FullName))
            {
                pages.Add(sr.ReadToEnd());
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
        audioSource.Play();
        currentPage += 2;
        Page1.text = pages[currentPage];
        if (currentPage + 1 < pages.Count)
        {
            Page2.text = pages[currentPage + 1];
        }
        if (currentPage < pages.Count - 1)
        {
            NextButton.SetActive(false);
        }
        if (currentPage >0)
        {
            BackButton.SetActive(true);
        }
    }
    public void OnBackClicked()
    {
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
            StartCoroutine(MoveCamera(startPosition));
        }
        else
        {
            StartCoroutine(MoveCamera(focusPosition));
        }
    }
    IEnumerator MoveCamera(Vector3 target)
    {
        Vector3 startPosition = trans.position;
        for (int i = 0; i < 50; i++)
        {
            trans.position = Vector3.Lerp(startPosition, target, i / 50f);
            yield return null;
        }
        focused = !focused;
    }
}
