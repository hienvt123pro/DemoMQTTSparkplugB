using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoMQTTSparkplugB.Model
{
    class DataMetricVoltNBIRTH
    {
        public string Name { get; set; }
        public double Timestamp { get; set; }
        public string DataType { get; set; }
        public double Value { get; set; }
    }
}
