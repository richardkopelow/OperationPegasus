using Standard;

public class Multiplier
{
    public float Product(float x, float y)
    {

    }
}
public class Test : TestBed
{
    public override void Run()
    {
        Multiplier mult = new Multiplier();
        Console.WriteLine("The product of 2 and 3 is: "+mult.Product(2,3));
        Console.WriteLine("The product of 5 and 6 is: " + mult.Product(5, 6));
        Console.WriteLine("The product of 20 and 80 is: " + mult.Product(20, 80));
    }
}
