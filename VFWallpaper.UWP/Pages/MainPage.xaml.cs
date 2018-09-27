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
            
            Log($"File picked: {pickedFile.Name}");

            // TODO: Make configurable 
            const double speed = 4;
            const int delay = 1;
            var offset = TimeSpan.FromMinutes(2);

            var resolution = ApplicationView.GetForCurrentView().VisibleBounds;
            var fe = new FrameExtractor(pickedFile, resolution);

            // TODO: Do this stuff in a task
            for (int i = 10; i < 100; i++)
            {
                Log($"Start extracting frame: {i}");
                var file = await fe.ExtractFrame(TimeSpan.FromSeconds(i * speed) + offset);

                Log($"Set as background");
                await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(file);

                Log($"Wait {delay} seconds");
                await Task.Delay(TimeSpan.FromSeconds(delay));
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
