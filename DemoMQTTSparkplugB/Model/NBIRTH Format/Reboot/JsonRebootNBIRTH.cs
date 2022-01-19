using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DemoMQTTSparkplugB.Model
{
    class JsonRebootNBIRTH
    {
        public string RebootNBIRTH(string name, double timestamp, string datatype, bool value)
        {
            var obj = new DataMetricRebootNBIRTH()
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
