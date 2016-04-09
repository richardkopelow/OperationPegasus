using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

class MissionManager
{
    private static MissionManager _instance;

    public static MissionManager Instance
    {
        get
        {
            if (_instance==null)
            {
                _instance = new MissionManager();
            }

            return _instance;
        }
    }

    public static Mission CurrentMission { get; set; }

    public void CheckSolution(Assembly assembly)
    {
        bool win=CurrentMission.Checker.CheckAnswer(assembly);
    }
}
