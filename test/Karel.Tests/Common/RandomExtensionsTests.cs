using Xunit;
using Karel.Common;

public class RandomExtensionsTests
{
    [Fact]
    public void Next_Int_ReturnsValueInRange()
    {
        var rng = new Random(0);
        int min = 10;
        int max = 20;
        for (int i = 0; i < 100; i++)
        {
            // Forces call to extension method versus non-generic Random.Next(int, int)
            int value = rng.Next<int>(min, max);
            Assert.InRange(value, min, max - 1);
        }
    }

    [Fact]
    public void Next_Double_ReturnsValueInRange()
    {
        var rng = new Random(0);
        double min = 5.5;
        double max = 10.5;
        for (int i = 0; i < 100; i++)
        {
            double value = rng.Next(min, max);
            Assert.InRange(value, min, max);
        }
    }

    [Fact]
    public void Next_Decimal_ReturnsValueInRange()
    {
        var rng = new Random(0);
        decimal min = 1.1m;
        decimal max = 2.2m;
        for (int i = 0; i < 100; i++)
        {
            decimal value = rng.Next(min, max);
            Assert.InRange(value, min, max);
        }
    }

    [Fact]
    public void Next_UInt32_ReturnsValueInRange()
    {
        var rng = new Random(0);
        uint min = 100;
        uint max = 200;
        for (int i = 0; i < 100; i++)
        {
            uint value = rng.Next(min, max);
            Assert.InRange(value, min, max - 1);
        }
    }
}