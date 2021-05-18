using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kmd.Logic.Cpr.Events.Receiver.Hubs;
using Kmd.Logic.Cpr.Events.Receiver.Models;
using Microsoft.AspNetCore.SignalR;

namespace Kmd.Logic.Cpr.Events.Receiver.Services
{
    public class EventService
    {
        private readonly IHubContext<CprEventsHub> hubContext;
        private List<CprEventListViewModel> cprEvents = new List<CprEventListViewModel>();

        public bool RejectEventsMode { get; set; } = false;

        public EventService(IHubContext<CprEventsHub> hubContext)
        {
            this.hubContext = hubContext;
        }

        public Task<CprEventListViewModel[]> GetEventstAsync()
        {
            return Task.FromResult(this.cprEvents.ToArray());
        }

        public async Task AddEventAsync(CprEvent cprEvent)
        {
            this.cprEvents.Add(new CprEventListViewModel { Time = DateTime.Now, CprEvent = cprEvent });
            await this.hubContext.Clients.All.SendAsync("RefreshData").ConfigureAwait(false);
        }

        public void ClearEvents()
        {
            this.cprEvents.Clear();
        }
    }
}
