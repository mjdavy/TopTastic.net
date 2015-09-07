using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TopTastic.Model
{
    public class MockDataService : IDataService
    {
        public async void GetPlaylistData(Action<BBCTop40PlaylistData, Exception> callback)
        {
            var testFilePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "TestChart.html");
            BBCTop40PlaylistData playlistData = null;
            Exception err = null;
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(testFilePath);
                var data = await FileIO.ReadTextAsync(file);
            }
            catch(Exception ex)
            {
                err = ex;
            }
            callback(playlistData, err);
        }

        public void GetThumnails(Action<IList<string>, Exception> callback)
        {
            throw new NotImplementedException();
        }
    }
}
