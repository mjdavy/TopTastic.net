using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TopTastic
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void player_MediaEnded(object sender, RoutedEventArgs e)
        {

        }

        private void player_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void player_MediaOpened(object sender, RoutedEventArgs e)
        {

        }

        private void AdaptiveStates_CurrentStateChanged(object sender, VisualStateChangedEventArgs e)
        {
           
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                TextBox t = sender as TextBox;
                var searchText = t.Text;
                System.Diagnostics.Debug.WriteLine(string.Format("Got {0}", searchText));
                e.Handled = true;
                t.Visibility = Visibility.Collapsed;

                if (!string.IsNullOrEmpty(searchText))
                {
                    var vm = this.DataContext as ViewModel.MainViewModel;
                    vm.SearchYouTube(searchText);
                }
            }
        }

        private void SearchYouTube_Click(object sender, RoutedEventArgs e)
        {
            this.SearchBox.Visibility = Visibility.Visible;
            this.SearchBox.Focus(FocusState.Keyboard);
        }

        private void GeneralPlaylist_Click(object sender, RoutedEventArgs e)
        {
            this.SearchBox.Visibility = Visibility.Visible;
            this.SearchBox.Focus(FocusState.Keyboard);
        }
    }
}
