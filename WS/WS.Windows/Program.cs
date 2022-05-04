// See https://aka.ms/new-console-template for more information
using WS;

Console.WriteLine("Starting.....");

var server = LocalWebserver.CreateWebServer("http://192.168.121.99:8080");
await server.RunAsync();

Console.WriteLine("awaiting requests.....");