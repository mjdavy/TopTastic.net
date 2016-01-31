using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.Storage;
using Windows.Data.Xml.Dom;
using System.Threading.Tasks;

namespace TopTastic.tests
{
    [TestClass]
    public class EchoNestTests
    {
        public string ApiKey
        {
            get;
            private set;
        }

        [TestInitialize]
        public void Init()
        {
            InitAsync().Wait();
        }

        public async Task InitAsync()
        { 
            string echoNestApiKey = await LoadApiKey();
            ApiKey = echoNestApiKey;
        }

        public async Task<string> LoadApiKey()
        {
            var secrets = new Uri("ms-appx:///Assets/echonest_secrets.xml");
            StorageFile sFile = await StorageFile.GetFileFromApplicationUriAsync(secrets);
            XmlDocument doc =  await XmlDocument.LoadFromFileAsync(sFile);
            var nodes = doc.GetElementsByTagName("EchnoNestApiKey");
            return nodes[0].InnerText;
        }
    }
}
