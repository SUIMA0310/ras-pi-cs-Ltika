using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ras_pi_cs.Gpio;
using ras_pi_cs.Server.Hubs;
using ras_pi_cs.Shared;

namespace ras_pi_cs.Server
{
    public class Searvice : IHostedService
    {
        private readonly IHubContext<Hubs.LedHub> _hubContext;
        private readonly GpioController controller;
        private readonly GpioWatcher watcher;

        public Searvice(IHubContext<Hubs.LedHub> hub)
        {
            Console.WriteLine($"{DateTime.Now}  : Searvice._ctr");

            _hubContext = hub;

            var pin = new PinConfigration(14, true)
            {
                AutoClose = false
            };
            controller = new GpioController(pin);
            watcher = new GpioWatcher(pin);

            watcher.ValueChanged += async () =>
            {
                Console.WriteLine($"{DateTime.Now} : ValueChanged");
                var value = await Task.Run(() =>
                {
                    return controller.Value;
                });
                var status = new LedStatus()
                {
                    Value = value
                };
                await _hubContext.Clients.All.SendAsync("ChangeLedStatus", status);
            };
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{DateTime.Now}  : Searvice.StartAsync");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine($"{DateTime.Now}  : Searvice.StopAsync");
            watcher.Dispose();

            return Task.CompletedTask;
        }
    }
}
