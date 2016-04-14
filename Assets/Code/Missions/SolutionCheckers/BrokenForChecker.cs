using UnityEngine;
using System;
using System.Reflection;

public class BrokenForChecker : SolutionChecker
{
    public override bool CheckAnswer(Assembly program)
    {
        object broken = program.CreateInstance("BrokenFor");
        MethodInfo method = broken.GetType().GetMethod("SumTo", new Type[]{ typeof(int)});
        int number = UnityEngine.Random.Range(0, 100);
        int result = (int)method.Invoke(broken, new object[] { number });
        int answer  = 0;
        for (int i = 0; i < number; i++)
        {
            if (i%2==0)
            {
                answer += i;
            }
            
        }
        return result==answer;
    }
}
