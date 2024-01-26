
using DotNetCore2;

var runBackground = true;

if (!runBackground)
{
    Console.WriteLine("Starting common service");
    var serviceTask = CommonHostedService.StartHostedService();
    await serviceTask;

    Console.WriteLine("Finishing common service");
}
else
{
    Console.WriteLine("Starting background service");
    var serviceTask = BackgroundHostedService.StartHostedService();
    await serviceTask;

    Console.WriteLine("Finishing background service");
}

Console.ReadLine();