using CodeWars.CenturyCalculator;
using NUnit.Framework;

namespace CodeWars.Tests
{
    [TestFixture]
    public class CenturyCalculatorTests
    {
        [Test]
        public void Should_Return_Correct_Century()
        {
            Assert.AreEqual(18, CenturyConverter.CalculateCentury(1705));
            Assert.AreEqual(19, CenturyConverter.CalculateCentury(1900));
            Assert.AreEqual(17, CenturyConverter.CalculateCentury(1601));
            Assert.AreEqual(20, CenturyConverter.CalculateCentury(2000));
        }
    }
}
