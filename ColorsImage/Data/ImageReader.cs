using CliWrap;
using CliWrap.Models;
using CliWrap.Services;
using ColorsImage.Util.Logger;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ColorsImage.Data
{
    public class ImageReader
    {
        public ILogger Logger { get; set; }

        //ffmpeg -i input.mp4 -ss 00:00:20 -t 10 -r 1 -q:v 2 -f image2 pic-%03d.jpeg
        public void GetFrame(string path, TimeSpan start,string fps, string name)
        {
            var videoName = name ?? Path.GetFileNameWithoutExtension(path);
            var dir = $"./images/{videoName}";
            Directory.CreateDirectory(dir);
            var argumentStr = $"ffmpeg.exe -i \"{path}\" -ss {start} -r {fps} -q:v 2 -f image2 \"{dir}/pic_%5d.jpg\"";
            using (var cli = new Cli("cmd"))
            {
                var handler = new BufferHandler(
                    stdOut=>Logger.Log(stdOut),
                    stdErr=>Logger.Log(stdErr)
                    );
                var output = cli.Execute1Async(argumentStr,bufferHandler:handler);
                //output.GetAwaiter().GetResult();
            }
        }

        public Task<ExecutionOutput> GetFrameAsync(string path, TimeSpan start, string fps, string name, CancellationToken token=default(CancellationToken))
        {
            var videoName = name ?? Path.GetFileNameWithoutExtension(path);
            var dir = $"./images/{videoName}";
            Directory.CreateDirectory(dir);
            var argumentStr = $"ffmpeg.exe -i \"{path}\" -ss {start} -r {fps} -q:v 2 -f image2 \"{dir}/pic_%5d.jpg\"";
            using (var cli = new Cli("cmd"))
            {
                var handler = new BufferHandler(
                    stdOut => Logger.Log(stdOut),
                    stdErr => Logger.Log(stdErr)
                    );
                return cli.Execute1Async(argumentStr,cancellationToken:token, bufferHandler: handler);
            }
        }

        public int GetFrameCount(string fileName)
        {
            var extStr = Path.GetExtension(fileName).ToLower();
            switch (extStr)
            {
                case "mkv":
                    return int.Parse(GetMkvFrameCount(fileName));
                case "mp4":
                    return int.Parse(GetMkvFrameCount(fileName));
                default:
                    throw new NotSupportedException("不支持的格式");
            }
        }

        public string GetMkvFrameCount(string fileName)
        {
            using (var cli = new Cli("./ffmpeg.exe"))
            {
                var output = cli.Execute($"-i {fileName}");
                var outStr = output.StandardOutput;
                if (string.IsNullOrEmpty(outStr))
                    outStr = output.StandardError;
                if (string.IsNullOrEmpty(outStr)) return null;
                return GetBeforeBr(outStr, "NUMBER_OF_FRAMES:");
            }
        }

        public string GetMp4FrameCount(string fileName)
        {
            using (var cli = new Cli("./ffprobe.exe"))
            {
                var output = cli.Execute($"-i {fileName} -show_streams -hide_banner");
                var sout = output.StandardOutput;
                if (!string.IsNullOrEmpty(sout))
                {
                    return GetBeforeBr(sout, "nb_frames=");
                }
            }
            return null;
        }

        private string GetBeforeBr(string str, string find)
        {
            var i1 = str.IndexOf(find) + find.Length;
            var i2 = str.IndexOf('\n', i1);
            return str.Substring(i1, i2 - i1);
        }

        public string Test()
        {
            using (var cli = new Cli("ipconfig"))
            {
                var output = cli.Execute("/all");
                var sout = output.StandardOutput;
                if (!string.IsNullOrEmpty(sout)) return sout;
                var eout = output.StandardError;
                if (!string.IsNullOrEmpty(sout)) return eout;
            }
            return null;
        }

    }
}
