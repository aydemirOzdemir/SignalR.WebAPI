using Covid19Chart.API.Services;
using Microsoft.AspNetCore.SignalR;

namespace Covid19Chart.API.Hubs;

public class CovidHub:Hub
{
    private readonly CovidService service;

    public CovidHub(CovidService service )
    {
        this.service = service;
    }
    public async Task GetCovidList()
    {
        await Clients.All.SendAsync("ReceiveCovidList",service.GetCovidChartList());
    }
}
