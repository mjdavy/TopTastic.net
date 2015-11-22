using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using EchoNest;
using Windows.Storage;
using EchoNest.Artist;
using Windows.Data.Xml.Dom;
using System.Threading.Tasks;
using System.IO;

namespace TopTastic.tests
{
    [TestClass]
    public class EchoNestTests
    {
        private EchoNestSession session;

        [TestInitialize]
        public void Init()
        {
            InitAsync().Wait();
        }

        public async Task InitAsync()
        { 
            string echoNestApiKey = await LoadApiKey();
            session = new EchoNestSession(echoNestApiKey);
        }

        public async Task<string> LoadApiKey()
        {
            var secrets = new Uri("ms-appx:///Assets/echonest_secrets.xml");
            StorageFile sFile = await StorageFile.GetFileFromApplicationUriAsync(secrets);
            XmlDocument doc =  await XmlDocument.LoadFromFileAsync(sFile);
            var nodes = doc.GetElementsByTagName("EchnoNestApiKey");
            return nodes[0].InnerText;
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
