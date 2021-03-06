using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DemoMQTTSparkplugB.Model
{
    class JsonScanNBIRTH
    {
        public string ScanNBIRTH(string name, double timestamp, string datatype, int value)
        {
            var obj = new DataMetricScanNBIRTH()
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
