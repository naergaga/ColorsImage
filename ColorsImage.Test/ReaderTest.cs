using System;
using ColorsImage.Data;
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
            int mode = 0;
            var path = "./video/Colors.mp4";
            reader.ReadAndWrite(path,mode,"A");
        }
    }
}
