using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using VFWallpaper.UWP.Utilities;

namespace VFWallpaper.UWP.BackgroundTasks
{
    public sealed class WallpaperChangerBackgroundTask : IBackgroundTask
    {
        private BackgroundTaskDeferral _deferral;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            // We need to use a deferral to make sure all awaits are done synchronous
            _deferral = taskInstance.GetDeferral();

            var bc = new WallpaperChanger();
            await bc.Update();

            _deferral.Complete();
        }
    }
}
