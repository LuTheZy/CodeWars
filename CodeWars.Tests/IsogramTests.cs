using System.Collections.Generic;
using NUnit.Framework;

namespace CodeWars.Tests
{
    [TestFixture]
    public class IsogramTests
    {
        private static IEnumerable<TestCaseData> TrueTestCases
        {
            get
            {
                yield return new TestCaseData("Dermatoglyphics").Returns(true);
                yield return new TestCaseData("isogram").Returns(true);
                yield return new TestCaseData("thumbscrewjapingly").Returns(true);
                yield return new TestCaseData("").Returns(true);
            }
        }

        private static IEnumerable<TestCaseData> FalseTestCases
        {
            get
            {
                yield return new TestCaseData("moose").Returns(false);
                yield return new TestCaseData("isIsogram").Returns(false);
                yield return new TestCaseData("aba").Returns(false);
                yield return new TestCaseData("moOse").Returns(false);
            }
        }

        [Test, TestCaseSource("TrueTestCases")]
        public bool Should_Return_True(string word)
        {
            return Isogram.IsIsogram(word);
        }

        [Test, TestCaseSource("FalseTestCases")]
        public bool Should_Return_False(string word)
        {
            return Isogram.IsIsogram(word);
        }
    }
}
    
