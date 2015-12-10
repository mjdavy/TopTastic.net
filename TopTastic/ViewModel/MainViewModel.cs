using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTastic.Model;
using Windows.UI.Xaml;

namespace TopTastic.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<PlaylistItemViewModel> _playlistItems;
        private PlaylistItemViewModel _selectedItem;
        private IDataService _service;
        private Uri _playerUri;
        private IPlaylistData _playlistData;
        private string _artistInfo;
        private Visibility _videoListProgress;
        private bool _videoLoading;
        private string _appbarStatusText;
        private int _appbarStatusValue;
        private Visibility _appbarStatusVisibility = Visibility.Collapsed;
        private bool _appbarStatusError;
        private bool _appbarStatusIndeterminate;
        private bool _playlistCreationInProgress;
        private bool _downloadMediaInProgress;

        #region Commands
        public RelayCommand CreateYoutubePlaylistCommand
        {
            get;
            private set;
        }

        public RelayCommand DownloadVideoCommand
        {
            get;
            private set;
        }

        public RelayCommand DownloadAudioCommand
        {
            get;
            private set;
        }


        void CreatCommands()
        {
            this.CreateYoutubePlaylistCommand = new RelayCommand(CreatePlaylist, CanCreatePlaylist);
            this.DownloadVideoCommand = new RelayCommand(DownloadVideo, CanDownloadVideo);
            this.DownloadAudioCommand = new RelayCommand(DownloadAudio, CanDownloadAudio);
        }

        async void LaunchUri(string videoId)
        {
            // The URI to launch
            var videoUri = new Uri("https://www.youtube.com/watch?v=" + videoId);

            // Launch the URI
            var success = await Windows.System.Launcher.LaunchUriAsync(videoUri);

            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
            }

        }

        void CreatePlaylist()
        {
            this.AppBarStatusVisibilty = Visibility.Visible;
            this.AppBarStatusText = "Creating YouTube Playlist";
            this.AppBarStatusIndeterminate = true;
            this.PlaylistCreationInProgress = true;
            _service.CreatePlaylist(_playlistData, (playlistid, err) =>
            {
                if (err == null)
                {
                    // MJDTODO
                    System.Diagnostics.Debug.WriteLine(playlistid);
                    this.AppBarStatusVisibilty = Visibility.Collapsed;
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                    this.AppBarStatusError = true;
                    this.AppBarStatusText = "Failed to create YouTube playlist.";
                }
                this.PlaylistCreationInProgress = false;
            });
        }

        void DownloadMedia(bool extractAudio)
        {
            this.AppBarStatusVisibilty = Visibility.Visible;
            this.AppBarStatusText = "Downloading Media";
            this.AppBarStatusIndeterminate = true;
            this.DownloadMediaInProgress = true;
            
            _service.DownloadMedia(this.PlayerUri, this.SelectedItem.Artist, this.SelectedItem.Title, extractAudio, (status, err) =>
            {
                if (err == null)
                {
                    // MJDTODO
                    System.Diagnostics.Debug.WriteLine(status);
                    this.AppBarStatusVisibilty = Visibility.Collapsed;
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                    this.AppBarStatusError = true;
                    this.AppBarStatusText = "Media download failed.";
                }
                this.DownloadMediaInProgress = false;
            });
        }
        void DownloadVideo()
        {
            DownloadMedia(false);
        }

        void DownloadAudio()
        {
            DownloadMedia(true);
        }


        bool CanCreatePlaylist()
        {
            return this.PlaylistItems != null && this.PlaylistItems.Count > 0 && this.PlaylistCreationInProgress == false;
        }

        bool CanDownloadAudio()
        {
            return this.CanDownloadVideo();
        }

        bool CanDownloadVideo()
        {
            return this.PlayerUri != null && this.DownloadMediaInProgress == false;
        }

        #endregion


        #region AppBar status

        public bool AppBarStatusIndeterminate
        {
            get
            {
                return _appbarStatusIndeterminate;
            }
            set
            {
                Set(() => AppBarStatusIndeterminate, ref _appbarStatusIndeterminate, value);
            }
        }

        public bool AppBarStatusError
        {
            get
            {
                return _appbarStatusError;
            }
            set
            {
                Set(() => AppBarStatusError, ref _appbarStatusError, value);
            }
        }
        public string AppBarStatusText
        {
            get
            {
                return _appbarStatusText;
            }
            set
            {
                Set(() => AppBarStatusText, ref _appbarStatusText, value);
            }
        }

        public int AppBarStatusValue
        {
            get
            {
                return _appbarStatusValue;
            }
            set
            {
                Set(() => AppBarStatusValue, ref _appbarStatusValue, value);
            }
        }

        public Visibility AppBarStatusVisibilty
        {
            get
            {
                return _appbarStatusVisibility;
            }
            set
            {
                Set(() => AppBarStatusVisibilty, ref _appbarStatusVisibility, value);
            }
        }
        #endregion

        public ObservableCollection<PlaylistItemViewModel> PlaylistItems
        {
            get
            {
                return _playlistItems;
            }
            set
            {
                Set(() => PlaylistItems, ref _playlistItems, value);
                CreateYoutubePlaylistCommand.RaiseCanExecuteChanged();
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
                if (value != null)
                {
                    Set(() => SelectedItem, ref _selectedItem, value);
                    PlayVideo(_service, value.VideoId);
                    UpdateArtistInfo(_service, value.Artist);
                }
            }
        }

        public PlaylistItemViewModel SelectedItemNarrow
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (value != null)
                {
                    Set(() => SelectedItemNarrow, ref _selectedItem, value);
                    LaunchUri(value.VideoId);
                }
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
                DownloadAudioCommand.RaiseCanExecuteChanged();
                DownloadVideoCommand.RaiseCanExecuteChanged();
            }
        }

        public Visibility VideoListProgress
        {
            get
            {
                return _videoListProgress;
            }
            set
            {
                Set(() => VideoListProgress, ref _videoListProgress, value);
            }
        }

        public bool VideosLoading
        {
            get
            {
                return _videoLoading;
            }
            set
            {
                Set(() => VideosLoading, ref _videoLoading, value);
                this.VideoListProgress = value ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public string ArtistInfo
        {
            get
            {
                return _artistInfo;
            }
            set
            {
                Set(() => ArtistInfo, ref _artistInfo, value);
            }
        }

        public bool PlaylistCreationInProgress
        {
            get
            {
                return _playlistCreationInProgress;
            }
            set
            {
                Set(() => PlaylistCreationInProgress, ref _playlistCreationInProgress, value);
                CreateYoutubePlaylistCommand.RaiseCanExecuteChanged();
            }
        }

        public bool DownloadMediaInProgress
        {
            get
            {
                return _downloadMediaInProgress;
            }
            set
            {
                Set(() => DownloadMediaInProgress, ref _downloadMediaInProgress, value);
                DownloadAudioCommand.RaiseCanExecuteChanged();
                DownloadVideoCommand.RaiseCanExecuteChanged();
            }
        }

        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                _service = new DataService();
            }
            else
            {
                _service = new DataService();
            }

            this.CreatCommands();
            this.InitializePlaylistItems(_service);

        }

        void InitializePlaylistItems(IDataService service)
        {
            this.VideosLoading = true;
            service.GetPlaylistData((playlistData, err) =>
            {
                if (err == null)
                {
                    var playlistItems = new ObservableCollection<PlaylistItemViewModel>();
                    foreach (var item in playlistData.Items)
                    {
                        playlistItems.Add(new PlaylistItemViewModel(item));
                    }
                    this._playlistData = playlistData;
                    this.PlaylistItems = playlistItems;
                    this.UpdateVideoInfo(service, playlistData);
                    
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                }
                this.VideosLoading = false;
            });
        }

        void UpdateVideoInfo(IDataService service, BBCTop40PlaylistData playlistData)
        {
            service.GetVideoInfo(playlistData,(videos, err) =>
            {
                if (err == null)
                {
                    foreach(var video in videos)
                    {
                        PlaylistItems[video.Index].VideoId = video.VideoId;
                        PlaylistItems[video.Index].Thumbnail =video.ThumbnailUrl;
                        PlaylistItems[video.Index].VideoId = video.VideoId;
                    }
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                }
            });
        }

        void UpdateArtistInfo(IDataService service, string artistQuery)
        {
            this.ArtistInfo = string.Empty;
            service.GetArtistInfo(artistQuery, (artistInfo, err) =>
            {
                if (err == null)
                {
                    this.ArtistInfo = artistInfo;
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
            this.PlayerUri = null;
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
