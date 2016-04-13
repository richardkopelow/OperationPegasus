using System;
using System.Reflection;
using System.IO;
using UnityEngine;

public class ShipmentManifestHackChecker : SolutionChecker
{
    public override bool CheckAnswer(Assembly program)
    {
        return false;
    }
}
