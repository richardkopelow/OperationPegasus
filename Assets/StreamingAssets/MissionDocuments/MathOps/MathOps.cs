using Standard;

public class MathOps
{
    public int Factorial(int number)
    {
        int res=1;
		for(int i=1;i<=number;i++)
		{
			res*=i;
		}
		return res;
    }
	public float Abs(float number)
	{
		if(number<0)
		return -1*number;
		
		return number;
	}
	public int Truncate(float number)
	{
		return (int)number;
	}
	public int Round(float number)
	{
		int trunc=Truncate(number);
		float diff=number-trunc;
		if(diff>=0.5f)
		{
			trunc++;
		}
		return trunc;
	}
	public bool IsPrime(int number)
	{
		if(number<0)
		return false;
		bool res=true;
		for(int i=2;i<number/2;i++)
		{
			if(number%i==0)
			{
				res=false;
			}
		}
		return res;
	}
}

public class Test : TestBed
{
    public override void Run()
    {
        //Put test code here, test cases or whatever else
    }
}
