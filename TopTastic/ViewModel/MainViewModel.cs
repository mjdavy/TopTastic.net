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


        #region Properties

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
                Set(() => SelectedItem, ref _selectedItem, value);
                if (value != null)
                {

                    if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Mobile")
                    {
                        var videoUri = new Uri("https://www.youtube.com/watch?v=" + value.VideoId);
                        LaunchUri(videoUri);
                    }
                    else
                    {
                        PlayVideo(_service, value.VideoId);
                        UpdateArtistInfo(_service, value.Artist);
                    }

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

        #region Commands

        public RelayCommand SearchCommand
        {
            get;
            private set;
        }
        public RelayCommand CreateNewPlaylistCommmand
        {
            get;
            private set;
        }
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

        private void CreatCommands()
        {
            this.CreateNewPlaylistCommmand = new RelayCommand(CreateNewPlayList, CanCreateNewPlaylist);
            this.SearchCommand = new RelayCommand(Search, CanSearch);
            this.CreateYoutubePlaylistCommand = new RelayCommand(SharePlaylistOnYouTube, CanCreatePlaylist);
            this.DownloadVideoCommand = new RelayCommand(DownloadVideo, CanDownloadVideo);
            this.DownloadAudioCommand = new RelayCommand(DownloadAudio, CanDownloadAudio);
        }

        private bool CanCreateNewPlaylist()
        {
            throw new NotImplementedException();
        }

        private void CreateNewPlayList()
        {
            throw new NotImplementedException();
        }

        private bool CanSearch()
        {
            return true;
        }

        //new Uri("https://www.youtube.com/playlist?list=" + playlistId);
        //new Uri("https://www.youtube.com/watch?v=" + videoId);

        async void LaunchUri(Uri uri)
        {
            // Launch the URI
            var success = await Windows.System.Launcher.LaunchUriAsync(uri);

            if (success)
            {
                // URI launched
            }
            else
            {
                // URI launch failed
            }

        }

        public void Search()
        {
            MessengerInstance.Send(new SearchMessage(), 0);
        }

        public void OnSearch(SearchMessage message)
        {

        }

        public void SearchYouTube(string artist, string title)
        {
            this.SelectedItem = null;
            this.ArtistInfo = null;
            this.AppBarStatusVisibilty = Visibility.Visible;
            this.AppBarStatusText = "Searching YouTube";
            this.AppBarStatusIndeterminate = true;
            _service.SearchYouTube(artist, title, (playlistData, err) =>
            {
                if (err == null)
                {
                    this.AppBarStatusVisibilty = Visibility.Collapsed;
                    this.UpdatePlaylist(_service, playlistData);
                    this.SelectedItem = this.PlaylistItems.FirstOrDefault();
                    this.PlayVideo(_service, SelectedItem.VideoId);
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                    this.AppBarStatusError = true;
                    this.AppBarStatusText = "YouTube search failed.";
                }
                
            });
        }
        public void CreateNewPlaylistFromSearchText(string searchText)
        {
            this.AppBarStatusVisibilty = Visibility.Visible;
            this.AppBarStatusText = "Creating YouTube Playlist";
            this.AppBarStatusIndeterminate = true;
            this.PlaylistCreationInProgress = true;
            _service.GetEchoNestPlaylistData(searchText, (playlistData, err) =>
            {
                if (err == null)
                {
                    this.AppBarStatusVisibilty = Visibility.Collapsed;
                    this.UpdatePlaylist(_service, playlistData);
                    
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                    this.AppBarStatusError = true;
                    this.AppBarStatusText = "Failed to generate playlist.";
                }
                this.PlaylistCreationInProgress = false;
            });
        }

        void SharePlaylistOnYouTube()
        {
            this.AppBarStatusVisibilty = Visibility.Visible;
            this.AppBarStatusText = "Sharing Playlist on your YouTube channel";
            this.AppBarStatusIndeterminate = true;
            this.PlaylistCreationInProgress = true;
            _service.SharePlaylistOnYouTube(_playlistData, (playlistId, err) =>
            {
                if (err == null)
                {
                    System.Diagnostics.Debug.WriteLine(playlistId);
                    this.AppBarStatusVisibilty = Visibility.Collapsed;
                    var playlistUri = new Uri("https://www.youtube.com/playlist?list=" + playlistId);
                    LaunchUri(playlistUri);
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                    this.AppBarStatusError = true;
                    this.AppBarStatusText = "Failed to share playlist";
                }
                this.PlaylistCreationInProgress = false;
            });
        }

        private void DownloadMedia(bool extractAudio)
        {
            this.AppBarStatusVisibilty = Visibility.Visible;
            this.AppBarStatusText = "Downloading Media";
            this.AppBarStatusIndeterminate = true;
            this.DownloadMediaInProgress = true;

            if (this.SelectedItem == null)
            {
                return;
            }

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
        public void DownloadVideo()
        {
            DownloadMedia(false);
        }

        public void DownloadAudio()
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

        #region Construction
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

        void UpdatePlaylist(IDataService service, PlaylistData playlistData)
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

        void InitializePlaylistItems(IDataService service)
        {
            this.VideosLoading = true;
            service.GetBBCPlaylistData((playlistData, err) =>
            {
                if (err == null)
                {
                    UpdatePlaylist(service, playlistData);
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                }
                this.VideosLoading = false;
            });
        }
        #endregion

        #region Methods
        void UpdateVideoInfo(IDataService service, PlaylistData playlistData)
        {
            service.GetVideoInfo(playlistData, (videos, err) =>
             {
                 if (err == null)
                 {
                     foreach (var video in videos)
                     {
                         PlaylistItems[video.Index].VideoId = video.VideoId;
                         PlaylistItems[video.Index].Thumbnail = video.ThumbnailUrl;
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
                    this.PlayerUri = youtubeUri;
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                }
            });

        }
        #endregion
    }
}
