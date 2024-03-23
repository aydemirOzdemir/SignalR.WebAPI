using Covid19Chart.API.Hubs;
using Covid19Chart.API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Covid19Chart.API.Services;

public class CovidService
{
    private readonly CovidAppDbContext context;
    private readonly IHubContext<CovidHub> hubContext;

    public CovidService(CovidAppDbContext context,IHubContext<CovidHub> hubContext)
    {
        this.context = context;
        this.hubContext = hubContext;
    }
    public IQueryable<Covid> GetList()
    {
        return context.Covids.AsQueryable();
    }
    public async Task SaveCovid(Covid covid)
    {
        await context.Covids.AddAsync(covid);
        await context.SaveChangesAsync();
        await hubContext.Clients.All.SendAsync("ReceiveCovidList",GetCovidChartList());
    }
    public List<CovidChart> GetCovidChartList()
    {
        List<CovidChart> covidCharts = new List<CovidChart>();

        using (var command = context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = "select tarih,[1],[2],[3],[4],[5] FROM (Select [City],[Count],Cast([CovidDate] as date) as tarih from Covid) as CovidT Pivot (Sum(Count) for City In ([1],[2],[3],[4],[5]) ) as ptable order by tarih asc";

            command.CommandType=System.Data.CommandType.Text;
            context.Database.OpenConnection();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    CovidChart chart = new CovidChart();
                    chart.CovidDate = reader.GetDateTime(0).ToShortDateString();

                    Enumerable.Range(1, 5).ToList().ForEach(x => 
                    {
                        if (System.DBNull.Value.Equals(reader[x]))
                        {
                            chart.Counts.Add(0);
                        }
                        else
                        {
                            chart.Counts.Add(reader.GetInt32(x));
                        }
                    });


                    covidCharts.Add(chart);


                }

            }

            context.Database.CloseConnection();
            return covidCharts;
        }

    }
}
