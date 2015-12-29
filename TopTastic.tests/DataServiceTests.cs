using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTastic.Model;

namespace TopTastic.tests
{
    [TestClass]
    public class DataServiceTests
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void TestCreateFileName()
        {
            string artist = "Justin Bieber";
            string title = "What do you mean?";
            string extension = ".mp3";

            var service = new DataService();
            var actual = service.CreateFileName(artist, title, extension);

            var expected = "Justin Bieber - What do you mean.mp3";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async void GetMockPlaylistData()
        {
            var ds = new MockDataService();
            BBCTop40PlaylistData result = null;

            ds.GetPlaylistData((playlistData, err) =>
            {
                if (err == null)
                {
                    var expected = result.Items.Count;
                    Assert.AreEqual(40, expected);
                }
                else
                {
                    
                }
            });
        }

        
    }
}
