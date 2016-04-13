using Standard;

public class BrokenFor
{
    public int SumTo(int number)
    {
        int result = 0;
        for (int i = 0; i < number; i++
        {
            result += i;
        }
        return result;
    }
}
public class Test : TestBed
{
    public override void Run()
    {
        BrokenFor bf = new BrokenFor();
        Console.WriteLine("Testing number 3: " + bf.SumTo(3));
        Console.WriteLine("Testing number 9: " + bf.SumTo(9));
        Console.WriteLine("Testing number 16: " + bf.SumTo(16));
    }
}
