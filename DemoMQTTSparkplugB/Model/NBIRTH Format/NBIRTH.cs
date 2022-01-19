using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DemoMQTTSparkplugB.Model
{
    public class NBIRTH
    {
        public string JsonNBIRTH (double timestamp, string metrics, int seq)
        {
            var obj = new DataNBIRTH()
            {
                TimeStamp = timestamp,
                Metrics = metrics,
                Seq = seq
            };
            return JsonConvert.SerializeObject(obj);
        }   

    }
}
