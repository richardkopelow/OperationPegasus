using Standard;

public class MathOps
{
    public int Factorial(int number)
    {
        //calculate the factorial
		int factorial = 1;
        for (int i = 2; i <= number; i++)
        {
            factorial *= i;
        }
		return factorial;
    }
}

public class Test : TestBed
{
    public override void Run()
    {
        //Put test code here, test cases or whatever else
		MathOps m=new MathOps();
		Console.WriteLine(m.Factorial(4).ToString());
    }
}
