using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using EchoNest;
using Windows.Storage;
using EchoNest.Artist;

namespace TopTastic.tests
{
    [TestClass]
    public class EchoNestTests
    {
        private EchoNestSession session;

        [TestInitialize]
        public void Init()
        {
            var echoNestApiKey = "UTXDWPYJJJ8BBB8OH";

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["EchoNestApiKey"] = echoNestApiKey;

            session = new EchoNestSession(echoNestApiKey);
        }

        [TestMethod]
        public void TestArtistBio()
        {
            string query = "Adele";
            var result = session.Query<Biography>().Execute(query, numberOfResults: 1);
            var expected = 1;
            var actual = result.Biographies.Count;
            Assert.AreEqual(expected, actual);
        }
    }
}
