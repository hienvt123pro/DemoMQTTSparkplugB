using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DemoMQTTSparkplugB.Model
{
    class NDATA
    {
        public string JsonNDATA(double timestamp, string metrics, int seq)
        {
            var obj = new DataNDATA()
            {
                TimeStamp = timestamp,
                Metrics = metrics,
                Seq = seq
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}
