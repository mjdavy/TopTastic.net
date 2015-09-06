using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.IO;

namespace YouTubeTop40.Tests
{
    [TestClass]
    public class YouTubeHelperTests
    { 
        [TestMethod]
        public void TestRemoveOrdinals1()
        {
            string testString = "1st,2nd,3rd,4th,10th";
            string actual = BBCTop40PlaylistSource.RemoveOrdinalsFromDateString(testString);
            string expected = "1,2,3,4,10";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRemoveOrdinals2()
        {
            string testString = "14th August 2015";
            string actual = BBCTop40PlaylistSource.RemoveOrdinalsFromDateString(testString);
            string expected = "14 August 2015";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestRemoveOrdinals3()
        {
            string testString = "14th August 2015 or 21nd December2016";
            string actual = BBCTop40PlaylistSource.RemoveOrdinalsFromDateString(testString);
            string expected = "14 August 2015 or 21 December2016";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestExtractDate()
        {
            string testString = @"<leading><title>The Official UK Top 40 Singles Chart - 3rd November 2013</title><trailing>";
            DateTime actual = BBCTop40PlaylistSource.ExtractDate(testString);
            DateTime expected = new DateTime(2013, 11, 3);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestExtractSearchKeys()
        {
            string testPage;
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("YouTubeTop40.Tests.TestChart.html");
            using (var sr = new StreamReader(stream))
            {
                testPage = sr.ReadToEnd();
            };

            var keys = BBCTop40PlaylistSource.ExtractSearchKeys(testPage);
            Assert.AreEqual(40, keys.Count);
        }
    }
}
