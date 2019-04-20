using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ras_pi_cs.Shared;

namespace ras_pi_cs.Server.Hubs
{
    public class LedHub : Hub
    {

        public async Task<LedStatus> GetLedStatus()
        {
            throw new NotImplementedException();
        }

        public async Task SetLedStatus(LedStatus status)
        {
            Console.WriteLine($"*************** SetLedStasus :{status.Value}");

            await Clients.All.SendAsync("ChangeLedStatus", new LedStatus()
            {
                Value = status.Value
            });
        }
    }
}