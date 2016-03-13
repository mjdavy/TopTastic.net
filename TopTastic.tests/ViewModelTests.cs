using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var vm = new MainViewModel();
            var searchText = "Justin Timberlake - My Love";

            var exists = await MediaFileExists(KnownLibraryId.Music, searchText + ".mp3");
            Assert.IsFalse(exists);

            vm.SearchYouTube(searchText);
            vm.DownloadAudio();

            exists = await MediaFileExists(KnownLibraryId.Music, searchText + ".mp3");
            Assert.IsTrue(exists);
        }

        public async Task<bool> MediaFileExists(KnownLibraryId libraryId, string fileName)
        {
            var lib = await StorageLibrary.GetLibraryAsync(libraryId);
            var result = await lib.SaveFolder.TryGetItemAsync(fileName);
            return result != null;
        }
    }
}
