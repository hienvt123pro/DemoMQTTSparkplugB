using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Server;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System.Windows.Threading;
using System.Threading;
using System.Windows;
namespace DemoMQTTSparkplugB.Model
{
    class ConnectServer1
    {
        private const string MQTTServerUrl = "localhost";
        private const int MQTTPort = 1883;

        public bool PermitPublish { get; set; }
        public bool IsPublish { get; set; }
        public bool IsChecked { get; set; }

        public Task<bool> CntServerAndPublish (string CurrentTopic, string CurrentPayload)
        {
            MqttFactory factory1 = new MqttFactory();
            var mqttClient1 = factory1.CreateMqttClient();             
            var option1 = new MqttClientOptionsBuilder().WithTcpServer(MQTTServerUrl, MQTTPort).WithCredentials("mqtt", "matkhau123").Build();
            _ = mqttClient1.ConnectAsync(option1, CancellationToken.None);

            mqttClient1.UseConnectedHandler(async a =>
            {
                IsChecked = true;

                if (IsPublish == true)
                {
                    var message = new MqttApplicationMessageBuilder().WithTopic(CurrentTopic).WithPayload(CurrentPayload).Build();
                    await mqttClient1.PublishAsync(message);
                    IsPublish = false;
                }                
            });
     
            mqttClient1.UseDisconnectedHandler( async a =>
            {
                IsChecked = false;
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await mqttClient1.ConnectAsync(option1, CancellationToken.None);
                }
                catch
                {
                    IsChecked = false;
                }
            });         
            return Task.FromResult((IsChecked));
        }
    }
}
