using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

public class GreatestSmallestChecker : SolutionChecker
{
    public override bool CheckAnswer(Assembly program)
    {
        object greatSmall = program.CreateInstance("GreatestSmallest");
        MethodInfo greatest = greatSmall.GetType().GetMethod("Smallest", new Type[] { typeof(int) });
        MethodInfo smallest = greatSmall.GetType().GetMethod("Smallest", new Type[] { typeof(int) });

        if ((int)smallest.Invoke(greatSmall, new object[] { 3, 4, 13 }) !=24) 
        {
            return false;
        }
        if ((int)greatest.Invoke(greatSmall, new object[] { 3, 4, 13 }) != 12)
        {
            return false;
        }
        if ((int)smallest.Invoke(greatSmall, new object[] { 20, 200, 200 }) != 400)
        {
            return false;
        }
        if ((int)greatest.Invoke(greatSmall, new object[] { 20, 10, 40 }) != 40)
        {
            return false;
        }
        return true;
    }
}