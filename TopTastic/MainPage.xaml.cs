using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

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

    }
}
