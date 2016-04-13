using System;
using System.Reflection;

public class MathOpsChecker : SolutionChecker
{
    public override bool CheckAnswer(Assembly program)
    {
        bool result=true;
        object adder = program.CreateInstance("MathOps");

        MethodInfo abs = adder.GetType().GetMethod("Abs", new Type[] { typeof(float) });
        float number4 = UnityEngine.Random.Range(0f, 100f);
        float absed = (float)abs.Invoke(adder, new object[] { number4 });
        if (absed != Math.Abs(number4))
        {
            result = false;
        }

        MethodInfo method = adder.GetType().GetMethod("Truncate", new Type[] { typeof(float) });
        float number = UnityEngine.Random.Range(0f, 100f);
        int trunked = (int)method.Invoke(adder, new object[] { number });
        if (trunked!=(int)number)
        {
            result = false;
        }

        MethodInfo method2 = adder.GetType().GetMethod("Round", new Type[] { typeof(float) });
        float number2 = UnityEngine.Random.Range(0f, 100f);
        int rounded = (int)method2.Invoke(adder, new object[] { number2 });
        if (rounded != Math.Round(number2))
        {
            result = false;
        }

        MethodInfo method3 = adder.GetType().GetMethod("IsPrime", new Type[] { typeof(int) });
        for (int i = 0; i < 40; i++)
        {
            int number3 = UnityEngine.Random.Range(0, 100);
            bool prime = (bool)method3.Invoke(adder, new object[] { number3 });

            bool isPrime = true;
            for (int j = 2; j < number3; j++)
            {
                if (number3 % j == 0)
                {
                    isPrime = false;
                }
            }

            if (prime != isPrime)
            {
                result = false;
            }
        }
        bool prime2 = (bool)method3.Invoke(adder, new object[] { -4 });
        if (prime2)
        {
            result = false;
        }
        return result;
    }
}
