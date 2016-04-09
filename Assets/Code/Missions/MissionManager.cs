using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.IO;

class MissionManager:MonoBehaviour
{
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
        #endregion

        CurrentMission = Missions[missionNumber];

        missionManual = GameObject.Find("MissionManual").GetComponent<Manual>();
        SetupMission();
    }

    public void CheckSolution(Assembly assembly)
    {
        bool win=CurrentMission.Checker.CheckAnswer(assembly);
        if (win)
        {
            int nextMission = CurrentMission.MissionNumber + 1;
            PlayerPrefs.SetInt("CurrentMission", nextMission);
            CurrentMission = Missions[nextMission];

            SetupMission();
        }
    }

    public void SetupMission()
    {
        missionManual.Populate("missions/" + CurrentMission.MissionNumber);

        DirectoryInfo di = new DirectoryInfo(Application.dataPath+"/StreamingAssets/MissionDocuments/"+CurrentMission.MissionNumber);
        if (di.Exists)
        {
            foreach (FileInfo fi in di.GetFiles())
            {
                if (!fi.Extension.Contains("meta"))
                {
                    fi.CopyTo(Application.dataPath + "/StreamingAssets/Drives/Q/Documents/"+fi.Name);
                }
            }
        }
    }
}
