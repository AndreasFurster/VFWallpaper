using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using VFWallpaper.UWP.BackgroundTasks;

namespace VFWallpaper.UWP.Utilities
{
    class BackgroundTaskRegistrar
    {
        public void RegisterBackgroundChangerTask(int interval)
        {
            if (interval < 1)
                throw new ArgumentOutOfRangeException($"{nameof(interval)} should be 1 or higher");

            const string taskName = nameof(WallpaperChangerBackgroundTask);
            var registered = BackgroundTaskRegistration.AllTasks.Any(t => t.Value.Name == taskName);
            
            if(registered)
                return;
            
            var builder = new BackgroundTaskBuilder
            {
                Name = taskName,
                TaskEntryPoint = $"Tasks.{taskName}"
            };
            
            builder.SetTrigger(new MaintenanceTrigger((uint) interval, false));

        }
    }
}
