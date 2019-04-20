using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ras_pi_cs.Shared;
using ras_pi_cs.Gpio;

namespace ras_pi_cs.Server.Hubs
{
    public class LedHub : Hub
    {
        private readonly GpioController controller;
        private readonly GpioWatcher watcher;

        public LedHub()
        {
            var pin = new PinConfigration(14, true)
            {
                AutoClose = false
            };
            controller = new GpioController(pin);
            watcher = new GpioWatcher(pin);

            watcher.ValueChanged += async () => 
            {
                var value = await Task.Run(() =>
                {
                    return controller.Value;
                });
                var status = new LedStatus()
                {
                    Value = value
                };
                await Clients.All.SendAsync("ChangeLedStatus", status);
            };
        }

        public async Task<LedStatus> GetLedStatus()
        {
            var value = await Task.Run(() =>
            {
                return controller.Value;
            });
            return new LedStatus()
            {
                Value = value
            };
        }

        public async Task SetLedStatus(LedStatus status)
        {
            await Task.Run(() => 
            {
                controller.Value = status.Value;
            });
        }

        protected override void Dispose(bool disposing)
        {
            ((IDisposable)controller).Dispose();
            watcher.Dispose();
            base.Dispose(disposing);
        }
    }
}