using Covid19Chart.API.Models;
using Covid19Chart.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Chart.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CovidController : ControllerBase
{
    private readonly CovidService covidService;

    public CovidController(CovidService covidService)
    {
        this.covidService = covidService;
    }
    [HttpPost]
    public async Task<IActionResult> SaveCovid(Covid covid)
    {
        await covidService.SaveCovid(covid);
      
        return Ok(covidService.GetCovidChartList());
    }
    [HttpGet]
    public async Task<IActionResult> InitializeCovid()
    {
        Random rnd=new Random();
        Enumerable.Range(1, 10).ToList().ForEach(async x => {
        foreach (ECity item in Enum.GetValues(typeof(ECity)))
            {
                var newCovid = new Covid
                {
                    City = item,
                    Count = rnd.Next(100, 1000),
                    CovidDate = DateTime.Now.AddDays(x)
                };
                await covidService.SaveCovid(newCovid);
                System.Threading.Thread.Sleep(1000);
            }
        
        });
        return Ok("Datalar Veri tabanına kaydedildi.");
    }
}
