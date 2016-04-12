using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Mission
{
    public delegate void GenericDel();

    public SolutionChecker Checker { get; set; }
    public string Name { get; set; }
    public List<string> NextMissions { get; set; }
    public bool NeedsManual { get; set; }
    public bool Started { get; set; }
    public GenericDel SpecialSetup { get; set; }

    public Mission(string name)
    {
        Name = name;
        NextMissions = new List<string>();
        NeedsManual = true;
    }
}
