using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ColorsImage.ViewModels
{
    public class MainViewModel
    {
        public string BasePath { get; set; }

        internal static MainViewModel GetDefault()
        {
            
            if (!File.Exists("./config.json")) {
                var m1  = new MainViewModel { BasePath = "./images" };
                m1.WriteConfig();
                return m1;
            }else
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MainViewModel));
                using (var sr = new FileStream("./config.json", FileMode.Open))
                {
                    return ser.ReadObject(sr) as MainViewModel;

                }
            }
        }

        public void WriteConfig()
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MainViewModel));
            using (var sr = new FileStream("./config.json", FileMode.Create))
            {
                ser.WriteObject(sr, this);
            }
        }
    }

}
