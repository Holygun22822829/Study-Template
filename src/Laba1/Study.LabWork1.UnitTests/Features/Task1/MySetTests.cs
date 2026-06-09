using NUnit.Framework;
using Study.LabWork1.Features.Task1;

namespace Study.LabWork1.UnitTests.Features.Task1;

public class RgbaPixelTests
{
    [Test]
    public void Constructor_ShouldClampChannels()
    {
        var pixel = new RgbaPixel(300, -10, 128, 1.5);

        Assert.That(pixel.Red, Is.EqualTo((byte)255));
        Assert.That(pixel.Green, Is.EqualTo((byte)0));
        Assert.That(pixel.Blue, Is.EqualTo((byte)128));
        Assert.That(pixel.Alpha, Is.EqualTo(1.0));
    }

    [Test]
    public void ToString_ShouldReturnExpectedFormat()
    {
        var pixel = new RgbaPixel(0, 0, 255, 0.5);

        Assert.That(pixel.ToString(), Is.EqualTo("rgba(0, 0, 255, 0.5)"));
    }

    [Test]
    public void ToHex_ShouldReturnExpectedHex()
    {
        var pixel = new RgbaPixel(255, 0, 16, 1);

        Assert.That(pixel.ToHex(), Is.EqualTo("#FF0010"));
    }

    [Test]
    public void Addition_ShouldClampResult()
    {
        var first = new RgbaPixel(200, 100, 50, 0.6);
        var second = new RgbaPixel(100, 200, 250, 0.7);

        var result = first + second;

        Assert.That(result.Red, Is.EqualTo((byte)255));
        Assert.That(result.Green, Is.EqualTo((byte)255));
        Assert.That(result.Blue, Is.EqualTo((byte)255));
        Assert.That(result.Alpha, Is.EqualTo(1.0));
    }

    [Test]
    public void Equality_ShouldCompareByValue()
    {
        var first = new RgbaPixel(10, 20, 30, 0.5);
        var second = new RgbaPixel(10, 20, 30, 0.5);

        Assert.That(first == second, Is.True);
        Assert.That(first != second, Is.False);
    }
}
