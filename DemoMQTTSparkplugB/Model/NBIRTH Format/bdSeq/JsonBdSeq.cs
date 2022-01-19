using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DemoMQTTSparkplugB.Model
{
    class JsonBdSeq
    {
        public string bdSeq(string name, double timestamp, string datatype, int value)
        {
            var obj = new DatabdSeq()
            {
                Name = name ,
                Timestamp = timestamp,
                DataType = datatype ,
                Value = value 
            };
            return JsonConvert.SerializeObject(obj);
        }
    }
}
