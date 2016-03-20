using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopTastic.Model;
using Windows.UI.Xaml;

namespace TopTastic.ViewModel
{
    public class SearchViewModel : ViewModelBase
    {
        private bool _isOpen;
        private string _artist;
        private string _title;

        public SearchViewModel()
        {
            GoCommand = new RelayCommand(Go, CanGo);
            CancelCommand = new RelayCommand(Cancel);
        }
        public string Artist
        {
            get
            {
                return _artist;
            }
            set
            {
                Set(() => Artist, ref _artist, value);
                GoCommand.RaiseCanExecuteChanged();
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                Set(() => Title, ref _title, value);
                GoCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsOpen
        {
            get
            {
                return _isOpen;
            }
            set
            {
                Set(() => IsOpen, ref _isOpen, value);
            }
        }

        public RelayCommand GoCommand
        {
            get;
            private set;
        }

        public RelayCommand CancelCommand
        {
            get;
            private set;
        }

        public void Open(SearchMessage msg)
        {
            if (msg != null)
            {
                this.Artist = msg.Artist;
                this.Title = msg.Title;
            }
            this.IsOpen = true;
        }

        public void Go()
        {
            IsOpen = false;
            var msg = new SearchMessage() { Artist = this.Artist, Title = this.Title };
            MessengerInstance.Send(msg, 1);
        }

        public void Cancel()
        {
            IsOpen = false;
        }

        public bool CanGo()
        {
            return !(string.IsNullOrEmpty(Artist) && string.IsNullOrEmpty(Title));
        }
    }
}
