using CodeWars.JaggedArrays;
using NUnit.Framework;

namespace CodeWars.Tests
{
    public class JaggedArrayTests
    {
        [Test]
        public void JaggedArray_Should_Return_Correct_Classifications()
        {
            Assert.AreEqual(new[] { "Open", "Senior", "Open", "Senior" }, MemberClassifier.OpenOrSenior(new[] { new[] { 45, 12 }, new[] { 55, 21 }, new[] { 19, 2 }, new[] { 104, 20 } }));
            Assert.AreEqual(new[] { "Open", "Open", "Open", "Open" }, MemberClassifier.OpenOrSenior(new[] { new[] { 3, 12 }, new[] { 55, 1 }, new[] { 91, -2 }, new[] { 54, 23 } }));
            Assert.AreEqual(new[] { "Senior", "Open", "Open", "Open" }, MemberClassifier.OpenOrSenior(new[] { new[] { 59, 12 }, new[] { 45, 21 }, new[] { -12, -2 }, new[] { 12, 12 } }));
        }
    }
}
