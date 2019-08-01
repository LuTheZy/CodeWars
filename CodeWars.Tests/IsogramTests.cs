using System;
using System.Collections.Generic;
using CodeWars.Isograms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;

namespace CodeWars.Tests
{
    [TestClass]
    public class IsogramTests
    {
        private static IEnumerable<TestCaseData> TestCases
        {
            get
            {
                yield return new TestCaseData("Dermatoglyphics").Returns(true);
                yield return new TestCaseData("isogram").Returns(true);
                yield return new TestCaseData("moose").Returns(false);
                yield return new TestCaseData("isIsogram").Returns(false);
                yield return new TestCaseData("aba").Returns(false);
                yield return new TestCaseData("moOse").Returns(false);
                yield return new TestCaseData("thumbscrewjapingly").Returns(true);
                yield return new TestCaseData("").Returns(true);
            }
        }

        [TestMethod, TestCaseSource("TestCases")]
        public bool Test(string word)
        {
            return Isogram.IsIsogram(word);
        }
    }
}
    
