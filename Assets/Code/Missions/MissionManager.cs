using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

class MissionManager:MonoBehaviour
{
    public GameObject SpeechBox;
    public Text SpeechText;
    public AudioClip SpeechNoise;

    private static MissionManager _instance;
    public static MissionManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public Mission CurrentMission { get; set; }
    public List<Mission> Missions { get; set; }

    Manual missionManual;

    bool speechClicked=false;

    void Start()
    {
        _instance = this;

        int missionNumber = PlayerPrefs.GetInt("CurrentMission", 0);
        #region SetupMissions
        Missions = new List<Mission>();

        Mission mis0 = new Mission();
        mis0.MissionNumber = 0;
        mis0.Checker = new Mission0Checker();
        Missions.Add(mis0);

        Mission mis1 = new Mission();
        mis1.MissionNumber = 1;
        mis1.Checker = new Mission1Checker();
        Missions.Add(mis1);

        Mission mis2 = new Mission();
        mis2.MissionNumber = 2;
        mis2.Checker = new Mission2Checker();
        Missions.Add(mis2);
        #endregion

        CurrentMission = Missions[missionNumber];

        missionManual = GameObject.Find("MissionManual").GetComponent<Manual>();
        SetupMission();
    }
    void LateUpdate()
    {
        speechClicked = false;
    }

    public void CheckSolution(Assembly assembly)
    {
        StartCoroutine(checkSolution(assembly));
    }
    IEnumerator checkSolution(Assembly assembly)
    {
        bool win = CurrentMission.Checker.CheckAnswer(assembly);
        if (win)
        {
            yield return StartCoroutine(runSpeechScript(CurrentMission.MissionNumber,false));
            int nextMission = CurrentMission.MissionNumber + 1;
            PlayerPrefs.SetInt("CurrentMission", nextMission);
            CurrentMission = Missions[nextMission];
            
            SetupMission();
        }
    }
    IEnumerator runSpeechScript(int mission, bool start)
    {
        string path = Application.dataPath + "/StreamingAssets/Speech/" + mission;
        if (start)
        {
            path += "/start.txt";
        }
        else
        {
            path += "/end.txt";
        }
        FileInfo fi = new FileInfo(path);
        if (fi.Exists)
        {
            SpeechBox.SetActive(true);
            using (StreamReader sr = new StreamReader(path))
            {
                string[] lines = sr.ReadToEnd().Replace("\r", "").Split('\n');
                foreach (string line in lines)
                {
                    string[] splitRes = line.Split('~');
                    char[] speechChars = splitRes[1].ToCharArray();

                    string displayText = "";
                    foreach (char c in speechChars)
                    {
                        displayText += c;
                        SpeechText.text = string.Format("<color={0}>{1}</color>", splitRes[0], displayText);
                        yield return new WaitForSeconds(0.05f);
                    }
                    while (!speechClicked)
                    {
                        yield return null;
                    }
                }
            }
            SpeechBox.SetActive(false);
        }
    }

    public void SetupMission()
    {
        StartCoroutine(setupMission());
    }
    IEnumerator setupMission()
    {
        yield return StartCoroutine(runSpeechScript(CurrentMission.MissionNumber, true));

        missionManual.Populate("missions/" + CurrentMission.MissionNumber);

        DirectoryInfo di = new DirectoryInfo(Application.dataPath + "/StreamingAssets/MissionDocuments/" + CurrentMission.MissionNumber);
        if (di.Exists)
        {
            foreach (FileInfo fi in di.GetFiles())
            {
                if (!fi.Extension.Contains("meta"))
                {
                    fi.CopyTo(Application.dataPath + "/StreamingAssets/Drives/Q/Documents/" + fi.Name);
                }
            }
        }
    }

    public void OnSpeechClicked()
    {
        speechClicked = true;
    }
}
