using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMQTTSparkplugB.Model
{
    class DDATA
    {
        public string JsonDDATA(double timestamp, string metrics, int seq)
        {
            var obj = new DataDDATA()
            {
                TimeStamp = timestamp,
                Metrics = metrics,
                Seq = seq
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}
