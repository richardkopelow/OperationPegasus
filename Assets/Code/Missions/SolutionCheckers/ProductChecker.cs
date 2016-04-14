using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

public class ProductChecker : SolutionChecker
{
    public override bool CheckAnswer(Assembly program)
    {
        object broken = program.CreateInstance("Multiplier");
        MethodInfo method = broken.GetType().GetMethod("Product", new Type[] { typeof(float), typeof(float) });

        float f = (float)method.Invoke(broken, new object[] { 4f, 7f });
        if (f==28f)
        {
            return true;
        }
        return false;
    }
}