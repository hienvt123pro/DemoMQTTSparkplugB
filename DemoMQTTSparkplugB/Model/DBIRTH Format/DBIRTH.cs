using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DemoMQTTSparkplugB.Model
{
    class DBIRTH
    {
        public string JsonDBIRTH(double timestamp, string metrics, int seq)
        {
            var obj = new DataDBIRTH()
            {
                TimeStamp = timestamp,
                Metrics = metrics,
                Seq = seq
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}
