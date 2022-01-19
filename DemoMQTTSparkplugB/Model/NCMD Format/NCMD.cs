using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DemoMQTTSparkplugB.Model
{
    class NCMD
    {
        public string JsonNCMD(double timestamp, string metrics)
        {
            var obj = new DataNCMD()
            {
                TimeStamp = timestamp,
                Metrics = metrics,              
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}
