using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.System.UserProfile;
using Windows.UI.ViewManagement;

namespace VFWallpaper.UWP.Utilities
{
    class BackgroundChanger
    {
        public async Task Update()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

            var path = (string) localSettings.Values["BackgroundVideoPath"];
            var speed = (int) localSettings.Values["BackgroundVideoSpeed"];
            var offsetStart = TimeSpan.FromSeconds((double) localSettings.Values["BackgroundVideoOffsetStart"]);
            var offsetEnd = TimeSpan.FromSeconds((double) localSettings.Values["BackgroundVideoOffsetEnd"]);
            var currentLocation = TimeSpan.FromSeconds((double) localSettings.Values["BackgroundVideoOffsetCurrentLocation"]);

            var frameTime = currentLocation;
            frameTime += TimeSpan.FromSeconds(speed);

            var video = await StorageFile.GetFileFromPathAsync(path);


            var di = DisplayInformation.GetForCurrentView();
            var resolution = new Rect(0,0, di.ScreenWidthInRawPixels, di.ScreenHeightInRawPixels);

            var fe = new FrameExtractor(video, resolution);
            var file = await fe.ExtractFrame(frameTime);

            await UserProfilePersonalizationSettings.Current.TrySetWallpaperImageAsync(file);

            localSettings.Values["BackgroundVideoOffsetCurrentLocation"] = frameTime.TotalSeconds;
        }
    }
}
