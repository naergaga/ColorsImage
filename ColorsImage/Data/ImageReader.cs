using Accord.Video.FFMPEG;
using ColorsImage.Util.Logger;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorsImage.Data
{
    public class ImageReader
    {
        public ILogger Logger { get; set; }

        public Bitmap Read()
        {
            VideoFileReader reader = new VideoFileReader();
            reader.Open("./video/Colors.mp4");
            return reader.ReadVideoFrame(1000);
        }

        public void ReadAndWrite(string path, int mode, string name)
        {
            var videoName = Path.GetFileNameWithoutExtension(path);
            Directory.CreateDirectory($"images/{videoName}");

            VideoFileReader reader = new VideoFileReader();
            reader.Open(path);
            for (int i = mode; i < reader.FrameCount; i += 101)
            {
                var fileName = $"images/{videoName}/{name}_{i:D5}_{mode:D3}.jpg";
                if (!File.Exists(fileName))
                    using (var bitmap = reader.ReadVideoFrame(i))
                    {
                        bitmap.Save(fileName, ImageFormat.Jpeg);
                        Logger?.Log($"保存图片 {fileName}");
                    }
            }
        }
    }
}
