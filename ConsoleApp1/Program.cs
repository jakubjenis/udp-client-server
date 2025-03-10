﻿// See https://aka.ms/new-console-template for more information

// UDP server address

using ConsoleApp1;

string address = "127.0.0.1";
if (args.Length > 0)
    address = args[0];

// UDP server port
int port = 3333;
if (args.Length > 1)
    port = int.Parse(args[1]);

Console.WriteLine($"UDP server address: {address}");
Console.WriteLine($"UDP server port: {port}");

Console.WriteLine();

// Create a new TCP chat client
var client = new MyUdpClient(address, port);

// Connect the client
Console.Write("Client connecting...");
client.Connect();
Console.WriteLine("Done!");

Console.WriteLine("Press Enter to stop the client or '!' to reconnect the client...");

// Perform text input
for (;;)
{
    string line = Console.ReadLine();
    if (string.IsNullOrEmpty(line))
        break;

    // Disconnect the client
    if (line == "!")
    {
        Console.Write("Client disconnecting...");
        client.Disconnect();
        Console.WriteLine("Done!");
        continue;
    }

    // Send the entered text to the chat server
    client.Send(line);
}

// Disconnect the client
Console.Write("Client disconnecting...");
client.DisconnectAndStop();
Console.WriteLine("Done!");