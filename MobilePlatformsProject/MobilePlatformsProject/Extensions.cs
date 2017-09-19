using Newtonsoft.Json.Linq;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MobilePlatformsProject
{
    public static class Extensions
    {
        public static bool FieldExists(this JObject jObject, string fieldName)
            => jObject[fieldName] != null;

        public static async Task Save(this UIElement grid)
        {
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(grid);
            var pixels = await renderTargetBitmap.GetPixelsAsync();


            var fileSavePicker = new FileSavePicker();
            fileSavePicker.FileTypeChoices.Add("BMP", new List<string>() { ".bmp" });
            fileSavePicker.FileTypeChoices.Add("GIF", new List<string>() { ".gif" });
            fileSavePicker.FileTypeChoices.Add("PNG", new List<string>() { ".png" });
            fileSavePicker.FileTypeChoices.Add("JPG", new List<string>() { ".jpg" });
            fileSavePicker.FileTypeChoices.Add("JPG-XR", new List<string>() { ".jxr" });
            fileSavePicker.FileTypeChoices.Add("TIFF", new List<string>() { ".tiff" });
            fileSavePicker.SuggestedFileName = SfChartResourceWrapper.FileName;

            var file = await fileSavePicker.PickSaveFileAsync();
            if (file != null)
            {
                Guid encoderId;
                switch (file.FileType)
                {
                    case ".jpg":
                    case ".jpeg":
                        encoderId = BitmapEncoder.JpegEncoderId;
                        break;
                    case ".jxr":
                        encoderId = BitmapEncoder.JpegXREncoderId;
                        break;
                    case ".png":
                        encoderId = BitmapEncoder.PngEncoderId;
                        break;
                    case ".tif":
                    case ".tiff":
                        encoderId = BitmapEncoder.TiffEncoderId;
                        break;
                    case ".bmp":
                    default:
                        encoderId = BitmapEncoder.BmpEncoderId;
                        break;
                }

                using (var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(encoderId, stream);
                    encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)renderTargetBitmap.PixelWidth,
                        (uint)renderTargetBitmap.PixelHeight, 600, 600, pixels.ToArray());
                    await encoder.FlushAsync();
                }
            }
        }
    }
}
