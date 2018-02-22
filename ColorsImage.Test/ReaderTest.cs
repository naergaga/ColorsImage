using System;
using ColorsImage.Data;
using ColorsImage.Util.Logger;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ColorsImage.Test
{
    [TestClass]
    public class ReaderTest
    {
        [TestMethod]
        public void Read()
        {
            ImageReader reader = new ImageReader();
            var path1 = "./video/Colors.mp4";
            var path2 = "./video/Colors01.mkv";
            Console.WriteLine(reader.GetMp4FrameCount(path1));
            Console.WriteLine(reader.GetMkvFrameCount(path2));

        }

        [TestMethod]
        public void GetFrames()
        {
            ImageReader reader = new ImageReader();
            var path1 = @"";
            reader.Logger = new Logger1((str) => Console.WriteLine(str));
            reader.GetFrame(path1, new TimeSpan(0, 20, 5), "1/60", "colors_01_1");
        }
    }
}
