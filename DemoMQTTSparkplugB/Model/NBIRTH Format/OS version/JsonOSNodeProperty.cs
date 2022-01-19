using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DemoMQTTSparkplugB.Model
{
    class JsonOSNodeProperty
    {
        public string OSNBIRTH(string name, double timestamp, string datatype, string value)
        {
            var obj = new DataMetricOSNBIRTH()
            {
                Name = name,
                Timestamp = timestamp,
                DataType = datatype,
                Value = value
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}
