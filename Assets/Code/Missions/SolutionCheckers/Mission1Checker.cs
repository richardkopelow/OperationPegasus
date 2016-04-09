using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

public class Mission1Checker : SolutionChecker
{
    public override bool CheckAnswer(Assembly program)
    {
        object adder = program.CreateInstance("MathOps");
        MethodInfo method = adder.GetType().GetMethod("Factorial", new Type[] { typeof(int) });
        int number = UnityEngine.Random.Range(0, 100);
        int result = (int)method.Invoke(adder, new object[] { number });

        int factorial = 1;
        for (int i = 2; i <= number; i++)
        {
            factorial *= i;
        }

        return result==factorial;
    }
}
