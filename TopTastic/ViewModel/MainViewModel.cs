﻿using GalaSoft.MvvmLight;
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

        void CreatePlaylist()
        {
            _service.CreatePlaylist(_playlistData, (playlistid, err) =>
            {
                if (err == null)
                {
                    // MJDTODO
                    System.Diagnostics.Debug.WriteLine(playlistid);
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                }
            });
        }

        void DownloadMedia(bool extractAudio)
        {
            _service.DownloadMedia(this.PlayerUri, this.SelectedItem.ArtistAndTitle, extractAudio, (status, err) =>
            {
                if (err == null)
                {
                    // MJDTODO
                    System.Diagnostics.Debug.WriteLine(status);
                }
                else
                {
                    /// if there is an error should create a property and bind to it for better practices
                    System.Diagnostics.Debug.WriteLine(err.ToString());
                }
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
            return this.PlaylistItems != null && this.PlaylistItems.Count > 0;
        }

        bool CanDownloadAudio()
        {
            return this.CanDownloadVideo();
        }

        bool CanDownloadVideo()
        {
            return this.PlayerUri != null;
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
                Set(() => SelectedItem, ref _selectedItem, value);
                PlayVideo(_service, value.VideoId);
                UpdateArtistInfo(_service, value.Artist);
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
