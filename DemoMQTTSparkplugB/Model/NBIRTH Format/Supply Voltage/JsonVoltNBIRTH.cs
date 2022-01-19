using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DemoMQTTSparkplugB.Model
{
    class JsonVoltNBIRTH
    {
        public string VoltNBIRTH(string name, double timestamp, string datatype, double value)
        {
            var obj = new DataMetricVoltNBIRTH()
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
