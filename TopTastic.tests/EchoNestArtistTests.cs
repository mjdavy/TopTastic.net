using EchoNest;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace TopTastic.tests
{
    [TestClass]
    public class EchoNestArtistTests : EchoNestTests
    {
        [TestMethod]
        public void TestArtistBio()
        {
            using (var session = new EchoNestSession(ApiKey))
            {
                string query = "Adele";
                var result = session.Query<EchoNest.Artist.Biography>().Execute(query, numberOfResults: 1);
                var expected = 1;
                var actual = result.Biographies.Count;
                Assert.AreEqual(expected, actual);
            }
        }

    }
}
