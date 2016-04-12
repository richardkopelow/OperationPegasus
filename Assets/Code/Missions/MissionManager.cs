using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

public class MissionManager:MonoBehaviour
{
    public GameObject SpeechBox;
    public Text SpeechText;
    public AudioClip TeletypeAudio;
    public AudioClip BeepAudio;
    public AudioClip PneumaticAudio;

    private static MissionManager _instance;
    public static MissionManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public List<Mission> CurrentMissions { get; set; }
    public Dictionary<string, Mission> Missions { get; set; }

    AudioSource audioSource;
    Transform missionManualTrans;
    Manual missionManual;

    bool speechClicked=false;

    void Start()
    {
        _instance = this;

        audioSource = GetComponent<AudioSource>();
        
        #region SetupMissions
        Missions = new Dictionary<string, Mission>();

        Mission addFives = new Mission("AddFives");
        addFives.Checker = new Mission0Checker();
        addFives.NextMissions.Add("Factorial");
        Missions.Add(addFives.Name, addFives);

        Mission factorial = new Mission("Factorial");
        factorial.Checker = new Mission1Checker();
        factorial.NextMissions.Add("MathOps");
        Missions.Add(factorial.Name, factorial);

        Mission mis2 = new Mission("MathOps");
        mis2.Checker = new Mission2Checker();
        mis2.NextMissions.Add("");
        Missions.Add(mis2.Name, mis2);
        #endregion

        string missionsString = PlayerPrefs.GetString("CurrentMissions");
        if (missionsString=="")
        {
            missionsString = "AddFives";
        }
        CurrentMissions = new List<Mission>();
        foreach (string missionName in missionsString.Split(','))
        {
            CurrentMissions.Add(Missions[missionName]);
        }

        GameObject manualGO = GameObject.Find("MissionManual");
        missionManualTrans = manualGO.GetComponent<Transform>();
        missionManual = manualGO.GetComponent<Manual>();
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
        List<Mission> missionsCompeted = new List<Mission>();
        foreach (Mission mission in CurrentMissions)
        {
            bool win = mission.Checker.CheckAnswer(assembly);
            if (win)
            {
                audioSource.clip = BeepAudio;
                audioSource.loop = false;
                audioSource.Play();
                while (audioSource.isPlaying)
                {
                    yield return null;
                }
                yield return StartCoroutine(runSpeechScript(mission.Name, false));

                missionsCompeted.Add(mission);
            }
        }
        foreach (Mission mis in missionsCompeted)
        {
            CurrentMissions.Remove(mis);

            foreach (string item in mis.NextMissions)
            {
                CurrentMissions.Add(Missions[item]);
            }
        }
        string currentMissionsString = "";
        foreach (Mission mission in CurrentMissions)
        {
            currentMissionsString += mission.Name + ",";
        }
        currentMissionsString = currentMissionsString.Substring(0, currentMissionsString.Length - 1);
        PlayerPrefs.SetString("CurrentMissions", currentMissionsString);
        SetupMission();
    }
    IEnumerator runSpeechScript(string mission, bool start)
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

                    audioSource.clip = TeletypeAudio;
                    audioSource.loop = true;
                    audioSource.Play();
                    string displayText = "";
                    foreach (char c in speechChars)
                    {
                        displayText += c;
                        SpeechText.text = string.Format("<color={0}>{1}</color>", splitRes[0], displayText);
                        yield return new WaitForSeconds(0.025f);
                    }
                    audioSource.Stop();
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
        List<string> manuals = new List<string>();
        foreach (Mission mission in CurrentMissions)
        {
            if (!mission.Started)
            {
                audioSource.clip = BeepAudio;
                audioSource.loop = false;
                audioSource.Play();
                while (audioSource.isPlaying)
                {
                    yield return null;
                }
                yield return StartCoroutine(runSpeechScript(mission.Name, true));

                if (mission.NeedsManual)
                {
                    manuals.Add("missions/" + mission.Name);
                }

                DirectoryInfo qDi = new DirectoryInfo(Application.dataPath + "/StreamingAssets/MissionDocuments/" + mission.Name+"/q");
                if (qDi.Exists)
                {
                    Debug.Log("found q");
                    copyMissionFiles("Q:",qDi);
                }
                DirectoryInfo tDi = new DirectoryInfo(Application.dataPath + "/StreamingAssets/MissionDocuments/" + mission.Name + "/t");
                if (tDi.Exists)
                {
                    copyMissionFiles("T:", tDi);
                }
                mission.Started = true;
            }
        }
        missionManual.Populate(manuals.ToArray());
        yield return StartCoroutine(runPneumatic());
    }
    void copyMissionFiles(string path, DirectoryInfo di)
    {
        foreach (DirectoryInfo subDi in di.GetDirectories())
        {
            copyMissionFiles(path + "/" + subDi.Name,subDi);
        }
        foreach (FileInfo fi in di.GetFiles())
        {
            if (!fi.Extension.Contains("meta"))
            {
                try
                {
                    Debug.Log("copy file "+fi.FullName+" to "+ path + "/" + fi.Name);
                    fi.CopyTo(path+"/" + fi.Name);
                }
                catch
                {
                }
            }
        }
    }
    IEnumerator runPneumatic()
    {
        audioSource.clip = PneumaticAudio;
        audioSource.loop = false;
        audioSource.Play();
        Vector3 start = missionManualTrans.position;
        Vector3 end = start + 5*Vector3.up;
        /*
        for (int i = 0; i < 40; i++)
        {
            missionManualTrans.position = Vector3.Lerp(start, end, i / 40f);
            yield return null;
        }
        */
        yield return new WaitForSeconds(2);
        /*
        for (int i = 0; i < 40; i++)
        {
            missionManualTrans.position = Vector3.Lerp(end, start, i / 40f);
            yield return null;
        }
        */
    }

    public void OnSpeechClicked()
    {
        speechClicked = true;
    }
}
