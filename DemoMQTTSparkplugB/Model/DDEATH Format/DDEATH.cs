using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DemoMQTTSparkplugB.Model
{
    class DDEATH
    {
        public string JsonDDEATH(double timestamp, int seq)
        {
            var obj = new DataDDEATH()
            {
                TimeStamp = timestamp,            
                Seq = seq
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}
