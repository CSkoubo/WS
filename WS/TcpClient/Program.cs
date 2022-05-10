using Core;

Console.WriteLine("Creating client");
DataClass sendDataClass = new DataClass { Number = 2, Text = "TextAAAAA" };
var serializer = new JsonSerializerWrapper();
var client = new ClientConnection(serializer, "127.0.0.1", 50001);
Console.WriteLine("client created");
Console.WriteLine("sending....");
await client.SendObjectAsync(sendDataClass);
Console.WriteLine("sent.");
