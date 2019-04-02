using System;
using System.Text;
using System.Threading;
using KubeMQ.SDK.csharp.Events;
using KubeMQ.SDK.csharp.Subscription;
using KubeMQ.SDK.csharp.Tools;

namespace Demo
{
    class Program
    {
        //The KubeMQ communication channel between pub sub
        const string eventChannelName = "TestCh_1";
        static void Main(string[] args)
        {

            //KubeMQ address can be set as environment parameter "KubeMQServerAddress"
            //The address port is the Grpc communication KubeMQ port 
            Console.WriteLine($"[Demo] KUBEMQ GRPC address:{Environment.GetEnvironmentVariable("KubeMQServerAddress")}");
    
            
            //Start event subscriber

            Console.WriteLine("[Demo] Starting demo SUB");
            //create KubeMQ.SDK.csharp.Subscription.SubscribeRequest
            SubscribeRequest subscribeRequest = new SubscribeRequest()
            {
                Channel = eventChannelName, //Subscribed channel name
                SubscribeType = SubscribeType.Events, //sub channel pattern
                ClientID = "sub_Demo"  //sub name ID
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
                goto End;
            }



            ///Starting sync publisher
            Console.WriteLine("[Demo] Starting demo PUB");          
            ChannelParameters eventChannelParameters = new ChannelParameters
            {
                //Publish channel name (must match the sub ChannelName)
                ChannelName = eventChannelName,
                //pub name ID
                ClientID = "pub_Demo"
            };



            //Create a new KubeMQ.SDK.csharp.Events.Channel 
            var eChannel = new Channel(eventChannelParameters);
            //KubeMQ.SDK.csharp.Events.Result is the pub message status.
            Result result = null;
            string msg = $"Hello there";
            try
            {   
                result = eChannel.SendEvent(new KubeMQ.SDK.csharp.Events.Event()
                {
                    //KubeMQ.SDK.csharp.ToolsConverter.ToByteArray, be sure do decode with the same encoder on sub.
                    Body = Converter.ToByteArray(msg)
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Demo] Could not send message, please check KubeMQ address, Error{ex.GetType()}");
                goto End;
            }

            if (!result.Sent)
            {
                Console.WriteLine($"[Demo] Could not send message:{result.Error}");
                goto End;
            }
           
            Console.WriteLine($"[Demo] Sent message ID:{result.EventID}");
            

            End:
            Console.WriteLine("[Demo] press Ctrl+c to stop");
            Console.ReadKey();           

        }
    }
}
