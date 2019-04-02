# kubemq-CSharp-pubsupDemo
Simple netcore Console app to demonstarate pub sub patteren with a single message

## Getting Started
The demo code contains an event subscriber and sender

### Prerequisites
* Make sure you have a KubeMQ running and can access the KubeMQ GRPC port.
* Create the environment varibale name "KubeMQServerAddress" and KubeMQ GRPC port as value.

### Subscriber
Subscriber recives and handles published Channel messages by a HandleEventDelegate.

```
  SubscribeRequest subscribeRequest = new SubscribeRequest()
            {
                Channel = eventChannelName, //Subscribed channel name
                SubscribeType = SubscribeType.Events, //sub channel pattern
                ClientID = "sub_Demo"  //sub name ID
            };

            //create a new KubeMQ.SDK.csharp.Events.Subscriber (KubeMQServerAddress environment var)
            Subscriber subscriber = new Subscriber();
           
          /// SubscribeToEvents HandleEventDelegate anonymous 
          subscriber.SubscribeToEvents(subscribeRequest, (eventReceive) =>
          {
              //KubeMQ.SDK.csharp.ToolsConverter.Converter.FromByteArray decode message from byte[]
              var recMsg = Converter.FromByteArray(eventReceive.Body);
              Console.WriteLine($"[Demo] Receive message ID:{eventReceive.EventID} received:{recMsg}");
          });
            
```
### Publisher
Publisher published messages to a Channel.
```
 ChannelParameters eventChannelParameters = new ChannelParameters
            {
                //Publish channel name (must match the sub ChannelName)
                ChannelName = eventChannelName,
                //pub name ID
                ClientID = "pub_Demo"
            };
            //Create a new KubeMQ.SDK.csharp.Events.Channel 
            var eChannel = new Channel(eventChannelParameters);          
            try
            {   
              var result = eChannel.SendEvent(new KubeMQ.SDK.csharp.Events.Event()
                {
                    //KubeMQ.SDK.csharp.ToolsConverter.ToByteArray, be sure do decode with the same encoder on sub.
                    Body = Converter.ToByteArray("Hello there")
                });
            }
```
