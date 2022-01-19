using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMQTTSparkplugB.Model
{
    class DCMD
    {
        public string JsonDCMD(double timestamp, string metrics)
        {
            var obj = new DataDCMD()
            {
                TimeStamp = timestamp,
                Metrics = metrics,
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}
