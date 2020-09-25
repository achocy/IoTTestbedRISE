using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoTTestbed.SFTPService
{
    public class MQTTClient
    {


        public static async Task ConnectAsync(int ExperimentId,string CurrentProjectName, List<int> SensorIDs)
        {
            string clientId = Guid.NewGuid().ToString();
            string mqttURI = "10.16.4.94";
            //string mqttUser = { REPLACE THIS WITH YOUR MQTT USER HERE }
            //string mqttPassword = { REPLACE THIS WITH YOUR MQTT PASSWORD HERE }
            int mqttPort = 1883;
            bool mqttSecure = false;
            var messageBuilder = new MqttClientOptionsBuilder()
              .WithClientId(clientId)
              //.WithCredentials(mqttUser, mqttPassword)
              .WithTcpServer(mqttURI, mqttPort)
              .WithCleanSession();
            var options = mqttSecure
              ? messageBuilder
                .WithTls()
                .Build()
              : messageBuilder
                .Build();
            var client = new MqttFactory().CreateMqttClient();
            await client.ConnectAsync(options);

            foreach (int uid in SensorIDs)
            {

             var message = new MqttApplicationMessageBuilder()
            .WithTopic("sensors/" + uid.ToString())
            .WithPayload("/home/pi/"+ ExperimentId.ToString() + "/" + CurrentProjectName.ToString() + "") /////////////////////////////////////
            .WithExactlyOnceQoS()
            .WithRetainFlag()
            .Build();

                await client.PublishAsync(message); // Since 3.0.5 with CancellationToken
            }
        }

    }
}