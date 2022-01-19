using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DemoMQTTSparkplugB.Model
{
    class JsonHardwareNodeProperty
    {
        public string HardwareNBIRTH(string name, double timestamp, string datatype, string value)
        {
            var obj = new DataMetricHardwareNBIRTH()
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
