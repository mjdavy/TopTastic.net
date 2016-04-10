using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using MyToolkit.Multimedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTastic.Model;
using TopTastic.ViewModel;
using Windows.Storage;

namespace TopTastic.tests
{
    [TestClass]
    public class ViewModelTests
    {
        
        [TestMethod]
        public async Task DownloadAudio()
        {
            var ds = new DataService();
            var artist = "Justin Timberlake";
            var title = "My Love";
            var fileName = string.Format("{0} - {1}.mp3", artist, title);

            var exists = await MediaFileExists(KnownLibraryId.Music, fileName);
            Assert.IsFalse(exists);

            var youTubeUri = await YouTube.GetVideoUriAsync("x1TsfShR5ZY", YouTubeQuality.QualityLow);
            await ds.DownloadMedia(youTubeUri.Uri, artist, title, true, null);

            exists = await MediaFileExists(KnownLibraryId.Music, fileName);
            Assert.IsTrue(exists);

            await DeleteFile(KnownLibraryId.Music, fileName);
            exists = await MediaFileExists(KnownLibraryId.Music, fileName);
            Assert.IsFalse(exists);

        }

        public async Task<bool> MediaFileExists(KnownLibraryId libraryId, string fileName)
        {
            var lib = await StorageLibrary.GetLibraryAsync(libraryId);
            var result = await lib.SaveFolder.TryGetItemAsync(fileName);
            return result != null;
        }

        public async Task DeleteFile(KnownLibraryId libraryId, string fileName)
        {
            var lib = await StorageLibrary.GetLibraryAsync(libraryId);
            var result = await lib.SaveFolder.TryGetItemAsync(fileName);

            if (result != null)
            {
                await result.DeleteAsync();
            }
        }

    }
}
