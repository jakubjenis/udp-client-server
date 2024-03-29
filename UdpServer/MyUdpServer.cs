using System.Net;
using System.Net.Sockets;

namespace UdpServer;

public class MyUdpServer(ILogger<MyUdpServer> logger, IPAddress address, int port)
    : NetCoreServer.UdpServer(address, port)
{
    protected override void OnStarted()
    {
        // Start receive datagrams
        ReceiveAsync();
    }

    protected override void OnReceived(EndPoint endpoint, byte[] buffer, long offset, long size)
    {
        //var stringMessage = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
        logger.LogDebug("Received message");
        ReceiveAsync();
    }
    
    protected override void OnError(SocketError error)
    {
        Console.WriteLine($"Echo UDP server caught an error with code {error}");
    }
}