using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTastic.Model;

namespace TopTastic.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<PlaylistItemViewModel> _playlistItems;
        private PlaylistItemViewModel _selectedItem;
        private IDataService _service;
        private Uri _playerUri;

        public ObservableCollection<PlaylistItemViewModel> PlaylistItems
        {
            get
            {
                return _playlistItems;
            }
            set
            {
                Set(() => PlaylistItems, ref _playlistItems, value);
            }
        }

        public PlaylistItemViewModel SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                Set(() => SelectedItem, ref _selectedItem, value);
                PlayVideo(_service, value.VideoId);
            }
        }

        public Uri PlayerUri
        {
            get
            {
                return _playerUri;
            }
            set
            {
                Set(() => PlayerUri, ref _playerUri, value);
            }
        }


        public MainViewModel()
        {
            _service = null;

            // MJDTODO - use DI
            //if (IsInDesignMode)
            //{
            //    service = new MockDataService();
            //}
            //else
            //{
            //    service = new DataService(); 
            //}
            _service = new DataService();
            this.InitializePlaylistItems(_service);
        }

        void InitializePlaylistItems(IDataService service)
        {
            service.GetPlaylistData((playlistData, err) =>
            {
                if (err == null)
                {
                    var playlistItems = new ObservableCollection<PlaylistItemViewModel>();
                    foreach (var item in playlistData.Items)
                    {
                        playlistItems.Add(new PlaylistItemViewModel(item));
                    }
                    this.PlaylistItems = playlistItems;
                    this.UpdateThumbnails(playlistData, service);
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                }
            });
        }

        void UpdateThumbnails(BBCTop40PlaylistData playlistData, IDataService service)
        {
            service.GetThumnails(playlistData,(thumbnails, err) =>
            {
                if (err == null)
                {
                    for(int i=0; i<PlaylistItems.Count; i++)
                    {
                        PlaylistItems[i].VideoId = thumbnails[i].Item1;
                        PlaylistItems[i].Thumbnail = thumbnails[i].Item2;
                    }
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                }
            });
        }

        void PlayVideo(IDataService service, string videoId)
        {
            service.GetYoutubeVideoUri(videoId, (youtubeUri, err) =>
            {
                if (err == null)
                {
                    this.PlayerUri = youtubeUri.Uri;
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                }
            });

        }
    }
}
