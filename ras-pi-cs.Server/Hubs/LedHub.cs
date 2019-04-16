using System;
using System.Linq;
using Microsoft.AspNetCore.SignalR;
using ras_pi_cs.Shared;

namespace ras_pi_cs.Server.Hubs
{
    public class LedHub : Hub
    {
        public LedStatus GetLedStatus()
        {
            throw new NotImplementedException();
        }

        public void SetLedStatus(LedStatus status)
        {
            throw new NotImplementedException();
        }
    }
}