using UdpServer;

var builder = Host.CreateApplicationBuilder(args);
builder.Services
    .AddHostedService<Worker>()
    .AddWindowsService(o =>
    {
        o.ServiceName = "UDP server";
    });

var host = builder.Build();
host.Run();