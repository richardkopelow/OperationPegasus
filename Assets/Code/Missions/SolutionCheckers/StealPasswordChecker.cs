using System;
using System.Reflection;
using System.IO;
using UnityEngine;

public class StealPasswordChecker : SolutionChecker
{
    public override bool CheckAnswer(Assembly program)
    {
        FileInfo fi = new FileInfo("T:/cred.txt");
        if (fi.Exists)
        {
            using (StreamReader sr=new StreamReader("T:/cred.txt"))
            {
                using (StreamReader answerReader=new StreamReader(Application.dataPath+"/StreamingAssets/MissionDocuments/StealPassword/PasswordFile.txt"))
                {
                    if (sr.ReadToEnd()==answerReader.ReadToEnd())
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
