using ColorsImage.Data;
using ColorsImage.Util.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ColorsImage.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ImageReader reader = new ImageReader();
            reader.Logger = new Logger1(msg => Console.WriteLine(msg));
            int mode = 0;
            var path = "./video/Colors.mp4";
            reader.ReadAndWrite(path, mode, "A");
        }
    }
}
