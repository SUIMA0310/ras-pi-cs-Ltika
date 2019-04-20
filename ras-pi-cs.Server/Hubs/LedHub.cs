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

        public LedHub()
        {
            //Console.WriteLine($"{DateTime.Now} : LedHub._ctr");

            var pin = new PinConfigration(14, true)
            {
                AutoClose = false
            };
            controller = new GpioController(pin);
        }

        public LedStatus GetLedStatus()
        {
            Console.WriteLine($"{DateTime.Now} : GetLedStatus");
            return new LedStatus()
            {
                Value = controller.Value
            };
        }

        public void SetLedStatus(LedStatus status)
        {
            Console.WriteLine($"{DateTime.Now} : SetLedStatus");
            controller.Value = status.Value;
        }

        protected override void Dispose(bool disposing)
        {
            //Console.WriteLine($"{DateTime.Now} : Dispose");
            ((IDisposable)controller).Dispose();
            base.Dispose(disposing);
        }
    }
}