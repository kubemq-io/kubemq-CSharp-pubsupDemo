# kubemq-CSharp-pubsupDemo
Simple net Console apps to demonstarate pub sub patteren.
Docker images are avilble on kubemq/kubemqsub_demo:latest/kubemq/kubemqpub_demo:latest

## Getting Started
The demo code contains 2 projects:
* KubeMQpub event publisher 
* KubeMQsub event subscriber

### Prerequisites
* Make sure you have a KubeMQ running and can access the KubeMQ GRPC port.
* Create the environment variable name "KubeMQServerAddress" and KubeMQ GRPC port as value.
* You can change the clientID name by environment variable CLIENT.
* You can change the clientID name by environment variable CHANNEL.
when testing KubeMQ locally can do this by cmd.exe environment variable

```
set KubeMQServerAddress=localaddress:50000
```

### Subscriber
Subscriber recives and handles published Channel messages by a HandleEventDelegate.

```
          //create KubeMQ.SDK.csharp.Subscription.SubscribeRequest
          SubscribeRequest subscribeRequest = new SubscribeRequest()
            {
                Channel = ChannelName, //Subscribed channel name
                SubscribeType = SubscribeType.Events, //sub channel pattern
                ClientID = ClientID  //sub name ID
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
                ChannelName = ChannelName,
                //pub name ID
                ClientID = ClientID
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
## Running Docker
plese change values [KubmqIp:GRPCPort] and CHANNEL=[ChannelName]
### Subscriber
```
docker run -i -t -e KubeMQServerAddress=[KubmqIp:GRPCPort] -e CHANNEL=[ChannelName]   kubemq/kubemqsub_demo:latest
```
### Publisher
```
docker run -i -t -e KubeMQServerAddress=[KubmqIp:GRPCPort] -e CHANNEL=[ChannelName]   kubemq/kubemqpub_demo:latest
```
