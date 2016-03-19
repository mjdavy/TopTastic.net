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
        private Visibility _visibility = Visibility.Visible;
        

        public SearchViewModel()
        {
            GoCommand = new RelayCommand(Go, CanGo);
            CancelCommand = new RelayCommand(Cancel);
        }
        public string Artist
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                Set(() => Visibility, ref _visibility, value);
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

        public void Go()
        {
            this.Visibility = Visibility.Collapsed;
            var msg = new SearchMessage() { Artist = this.Artist, Title = this.Title };
            MessengerInstance.Send<SearchMessage>(msg);
        }

        public void Cancel()
        {
            Visibility = Visibility.Collapsed;
        }

        public Boolean CanGo()
        {
            return !(string.IsNullOrEmpty(Artist) && string.IsNullOrEmpty(Title));
        }
    }
}
