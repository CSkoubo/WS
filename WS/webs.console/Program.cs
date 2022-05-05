// See https://aka.ms/new-console-template for more information
using webs.Services;

Console.WriteLine("Starting.....");

var client = new WebSocketClient();
await client.ConnectToServerAsync();
await client.SendMessageAsync("Hello from pc");
Console.WriteLine("awaiting requests.....");