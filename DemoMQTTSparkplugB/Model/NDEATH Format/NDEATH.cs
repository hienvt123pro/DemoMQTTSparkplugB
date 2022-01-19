using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DemoMQTTSparkplugB.Model
{
    class NDEATH
    {
        public string JsonNDEATH(double timestamp, string metrics)
        {
            var obj = new DataNDEATH()
            {
                TimeStamp = timestamp,
                Metrics = metrics,             
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}
