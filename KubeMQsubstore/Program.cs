using System;
using KubeMQ.SDK.csharp.Events;
using KubeMQ.SDK.csharp.Subscription;
using KubeMQ.SDK.csharp.Tools;

namespace KubeMQsubstore
{
    class Program
    {

        //The sub ClientID, can be set by environment var CLIENT
        private static string ClientID = Environment.GetEnvironmentVariable("CLIENT") ?? $"sub_Demo_{Environment.MachineName}";
        //The KubeMQ communication channel between pub sub, can be set by environment var CHANNEL 
        private static string ChannelName = Environment.GetEnvironmentVariable("CHANNEL") ?? "Test_Channel";

        static void Main(string[] args)
        {
            Console.WriteLine("[Demo] Starting demo SUB");
            //KubeMQ address environment var "KubeMQServerAddress" GRPC port         
            Console.WriteLine($"[Demo] KUBEMQ GRPC address:{Environment.GetEnvironmentVariable("KubeMQServerAddress")}");
            Console.WriteLine($"[Demo] ClientID:{ClientID}");
            Console.WriteLine($"[Demo] ChannelName:{ChannelName}");

            Console.WriteLine("[Demo] press Ctrl+c to stop");
            System.Threading.AutoResetEvent waitHandle = new System.Threading.AutoResetEvent(false);
            Console.CancelKeyPress += (object sender, ConsoleCancelEventArgs e) =>
            {
                e.Cancel = true;
                waitHandle.Set();
            };



            //create KubeMQ.SDK.csharp.Subscription.SubscribeRequest
            SubscribeRequest subscribeRequest = new SubscribeRequest()
            {
                Channel = ChannelName, //Subscribed channel name
                SubscribeType = SubscribeType.EventsStore, //sub channel pattern
                ClientID = ClientID,  //sub name ID
                EventsStoreType = EventsStoreType.StartFromFirst,
                EventsStoreTypeValue = 0

                
            };

            //create a new KubeMQ.SDK.csharp.Events.Subscriber (KubeMQServerAddress environment var)
            Subscriber subscriber = new Subscriber();
            try
            {
                /// SubscribeToEvents HandleEventDelegate anonymous 
                subscriber.SubscribeToEvents(subscribeRequest, (eventReceive) =>
                {
                    //KubeMQ.SDK.csharp.ToolsConverter.Converter.FromByteArray decode message from byte[]
                    var recMsg = Converter.FromByteArray(eventReceive.Body);
                    Console.WriteLine($"[Demo] Receive message ID:{eventReceive.EventID} received:{recMsg}");
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Demo] Did not subscribe, please check KubeMQ address, Error:{ex.GetType()}");
            }

            waitHandle.WaitOne();
        }
    }
}

