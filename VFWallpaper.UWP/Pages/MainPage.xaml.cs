using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.UserProfile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using VFWallpaper.UWP.Utilities;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VFWallpaper.UWP.Pages
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

        private async void Page_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {

            var pickedFile = await PickFile();

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["BackgroundVideoPath"] = pickedFile.Path;
            
            // TODO: Make configurable with popups or something
            const int speed = 4;
            const int delay = 15;
            var offsetStart = TimeSpan.FromMinutes(2);
            var offsetEnd = TimeSpan.FromMinutes(0);

            localSettings.Values["BackgroundVideoSpeed"] = speed;
            localSettings.Values["BackgroundVideoOffsetStart"] = offsetStart.TotalSeconds;
            localSettings.Values["BackgroundVideoOffsetEnd"] = offsetEnd.TotalSeconds;
            localSettings.Values["BackgroundVideoOffsetCurrentLocation"] = offsetStart.TotalSeconds;

            //var btc = new BackgroundTaskRegistrar();
            // btc.RegisterBackgroundChangerTask(15);

            for (int i = 0; i < 100; i++)
            {
                var bc = new WallpaperChanger();
                await bc.Update();

                await Task.Delay(TimeSpan.FromSeconds(5));

            }

        }

        private async Task<StorageFile> PickFile()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;

            picker.FileTypeFilter.Add(".mp4");
            picker.FileTypeFilter.Add(".mkv");

            return await picker.PickSingleFileAsync();
        }

        private void Log(string text)
        {
            LogTextBlock.Text += text + Environment.NewLine;
        }
    }
}
