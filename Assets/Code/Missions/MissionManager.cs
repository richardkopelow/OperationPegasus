using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;
using System.Diagnostics;

public class MissionManager:MonoBehaviour
{
    public GameObject SpeechBox;
    public Transform TubeDoor;
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
    Manual missionManual;

    bool speechClicked=false;

    void Start()
    {
        _instance = this;

        audioSource = GetComponent<AudioSource>();
        
        #region SetupMissions
        Missions = new Dictionary<string, Mission>();

        Mission addFives = new Mission("AddFives");
        addFives.Checker = new AddFivesChecker();
        addFives.NextMissions.Add("BrokenFor");
        Missions.Add(addFives.Name, addFives);

        Mission brokenFor = new Mission("BrokenFor");
        brokenFor.Checker = new BrokenForChecker();
        brokenFor.NextMissions.Add("Product");
        Missions.Add(brokenFor.Name, brokenFor);

        Mission product = new Mission("Product");
        product.Checker = new ProductChecker();
        product.NextMissions.Add("Factorial");
        Missions.Add(product.Name, product);

        Mission factorial = new Mission("Factorial");
        factorial.Checker = new FactorialChecker();
        factorial.NextMissions.Add("MathOps");
        factorial.NextMissions.Add("StealPassword");
        Missions.Add(factorial.Name, factorial);

        Mission mis2 = new Mission("MathOps");
        mis2.Checker = new MathOpsChecker();
        mis2.NextMissions.Add("GreatestSmallest");
        Missions.Add(mis2.Name, mis2);

        Mission stealPassword = new Mission("StealPassword");
        stealPassword.Checker = new StealPasswordChecker();
        stealPassword.NeedsManual = false;
        stealPassword.SpecialSetup = () =>
        {
            PlayerPrefs.SetInt("UnlockedT",1);

            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/c subst T: " + Application.dataPath + "/StreamingAssets/drives/t";
            process.StartInfo = startInfo;
            process.Start();
        };
        stealPassword.NextMissions.Add("ShipmentManifest");
        stealPassword.NextMissions.Add("ShipmentManifestHack");
        Missions.Add(stealPassword.Name, stealPassword);

        Mission greatestSmallest = new Mission("GreatestSmallest");
        greatestSmallest.Checker = new GreatestSmallestChecker();
        Missions.Add(greatestSmallest.Name, greatestSmallest);

        Mission shipmentManifest = new Mission("ShipmentManifest");
        shipmentManifest.Checker = new ShipmentManifestChecker();
        Missions.Add(shipmentManifest.Name, shipmentManifest);

        Mission shipmentManifestHack = new Mission("ShipmentManifestHack");
        shipmentManifestHack.Checker = new ShipmentManifestHackChecker();
        shipmentManifestHack.NeedsManual = false;
        Missions.Add(shipmentManifestHack.Name, shipmentManifestHack);
        #endregion

        string missionsString = PlayerPrefs.GetString("CurrentMissions");
        UnityEngine.Debug.Log(missionsString);
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
            bool win = false;
            try
            {
                win = mission.Checker.CheckAnswer(assembly);
            }
            catch
            {
            }
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

                DirectoryInfo qDi = new DirectoryInfo(Application.dataPath + "/StreamingAssets/MissionDocuments/" + mission.Name+"/q");
                if (qDi.Exists)
                {
                    copyMissionFiles("Q:",qDi);
                }
                DirectoryInfo tDi = new DirectoryInfo(Application.dataPath + "/StreamingAssets/MissionDocuments/" + mission.Name + "/t");
                if (tDi.Exists)
                {
                    copyMissionFiles("T:", tDi);
                }
                if (mission.SpecialSetup!=null)
                {
                    mission.SpecialSetup();
                }
                mission.Started = true;
            }

            if (mission.NeedsManual)
            {
                manuals.Add("missions/" + mission.Name);
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
        Vector3 start = TubeDoor.position;
        Vector3 end = start + -3.1f * Vector3.up;

        for (int i = 0; i < 40; i++)
        {
            TubeDoor.position = Vector3.Lerp(start, end, i / 40f);
            yield return null;
        }

        audioSource.clip = PneumaticAudio;
        audioSource.loop = false;
        audioSource.Play();
        yield return new WaitForSeconds(4);

        for (int i = 0; i < 40; i++)
        {
            TubeDoor.position = Vector3.Lerp(end, start, i / 40f);
            yield return null;
        }

    }

    public void OnSpeechClicked()
    {
        speechClicked = true;
    }
}
