using System.Threading.Tasks;

namespace TopTastic.Model
{
    public interface IPlaylistSource
    {
        Task<PlaylistData> GetPlaylistAsync();
    }
}