using UnityEngine;
using System;
using System.Reflection;

public class AddFivesChecker : SolutionChecker
{
    public override bool CheckAnswer(Assembly program)
    {
        object adder = program.CreateInstance("AddFives");
        MethodInfo method = adder.GetType().GetMethod("AddFive", new Type[]{ typeof(int)});
        int number = UnityEngine.Random.Range(0, 100);
        int result = (int)method.Invoke(adder, new object[] { number });
        return result==number+5;
    }
}
