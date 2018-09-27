using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Editing;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace VFWallpaper.UWP.Utilities
{
    class FrameExtractor
    {
        private Rect resolution;
        private StorageFile file;

        public FrameExtractor(StorageFile file, Rect resolution)
        {
            this.file = file;
            this.resolution = resolution;
        }

        public async Task<StorageFile> ExtractFrame(TimeSpan timeOfFrame)
        {

            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            var imageStream = await GetImageStream(timeOfFrame, file);
            return await SaveFrame(imageStream);
        }

        private async Task<ImageStream> GetImageStream(TimeSpan timeOfFrame, StorageFile pickedFile)
        {
            var clip = await MediaClip.CreateFromFileAsync(pickedFile);
            var composition = new MediaComposition();
            composition.Clips.Add(clip);

            var imageStream = await composition.GetThumbnailAsync(timeOfFrame, (int) resolution.Width, (int) resolution.Height,
                VideoFramePrecision.NearestFrame);
            return imageStream;
        }

        private async Task<StorageFile> SaveFrame(ImageStream imageStream)
        {
            //generate bitmap 
            var writableBitmap = new WriteableBitmap((int)resolution.Width, (int)resolution.Height);
            writableBitmap.SetSource(imageStream);

            //generate some random name for file in PicturesLibrary
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync("CurrentWallpaper_" + Guid.NewGuid().ToString().Substring(0, 4) + ".jpg");
            
            //get stream from bitmap
            Stream stream = writableBitmap.PixelBuffer.AsStream();
            byte[] pixels = new byte[(uint)stream.Length];
            await stream.ReadAsync(pixels, 0, pixels.Length);

            using (var writeStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, writeStream);
                encoder.SetPixelData(
                    BitmapPixelFormat.Bgra8,
                    BitmapAlphaMode.Premultiplied,
                    (uint)writableBitmap.PixelWidth,
                    (uint)writableBitmap.PixelHeight,
                    96,
                    96,
                    pixels);
                await encoder.FlushAsync();

                using (var outputStream = writeStream.GetOutputStreamAt(0))
                {
                    await outputStream.FlushAsync();
                }
            }

            return file;
        }
    }
}
