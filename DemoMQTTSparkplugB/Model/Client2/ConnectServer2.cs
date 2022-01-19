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
using DemoMQTTSparkplugB.Core;

namespace DemoMQTTSparkplugB.Model
{
    class ConnectServer2:ObservableObject
    {
        private const string MQTTServerUrl = "localhost";
        private const int MQTTPort = 1883;

        private string _message;

        public  string Message
        {
            get { return _message; }
            set { _message = value; OnPropertyChanged(); }
        }


        public bool IsChecked { get; set; }
    
        public Task<bool> CntServerAndSubcribe (string CurrentTopic)
        {
            MqttFactory factory2 = new MqttFactory();
            var mqttClient2 = factory2.CreateMqttClient();
            var option2 = new MqttClientOptionsBuilder().WithTcpServer(MQTTServerUrl, MQTTPort).WithCredentials("mqtt", "matkhau123").Build();
            mqttClient2.ConnectAsync(option2, CancellationToken.None);

            mqttClient2.UseConnectedHandler(async a =>
            {        
                // Subscribe to a topic
                await mqttClient2.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(CurrentTopic).Build());
                IsChecked = true;
            });

            mqttClient2.UseApplicationMessageReceivedHandler(a =>
            {
                Message = Encoding.UTF8.GetString(a.ApplicationMessage.Payload);        
            });

            mqttClient2.UseDisconnectedHandler(async a =>
            {
                IsChecked = false;
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await mqttClient2.ConnectAsync(option2, CancellationToken.None);
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
