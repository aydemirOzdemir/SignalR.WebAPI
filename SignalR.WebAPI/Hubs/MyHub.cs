using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using SignalR.WebAPI.Models;

namespace SignalR.WebAPI.Hubs;

public class MyHub:Hub
{
  
    private static List<string> Names = new();
    private readonly AppDbContext context;

    private static int ClientCount { get; set; } = 0;
    public MyHub(AppDbContext context)
    {
        this.context = context;
    }
    
    public async Task SendProduct(Product product)
    {
        await Clients.All.SendAsync("ReceiveProduct",product);
    }


    public async Task SendName(string name)
    {


        if (Names.Count >= 7)
            await Clients.Caller.SendAsync("Error", "Takım en fazla 7 kişi olabilir");
        else
        {
            Names.Add(name);
            await Clients.All.SendAsync("ReceiveName", name);
            //Clients propu Clientlarla iletişim kurabilmemizi sağlayan proptur. All tüm bağlı olan clientlara haberleşmeyı sağlar.SendAsync Clienttaki metotu tekiklememizi sağlar(ReceiveMessage).Clientlara geçilen mesaj.
        }
    }

    public async Task GetNames()
    {
        await Clients.All.SendAsync("ReceiveNames",Names);//Clientların Hepsini bilgilendir.
    }



    //Group a client ekleme
    public async Task AddToGroup(string teamName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId,teamName);
    }
    //gruptan client çıkarma
    public async Task RemoveToGroup(string teamName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId,teamName);
    }
    //grupa kişi ekleme
    public async Task SendNameByGroup(string name,string teamName)
    {
        Team team=context.Teams.FirstOrDefault(s => s.Name==teamName);
        if (team != null)
        {
             team.Users.Add(new User { Name = name });
            context.Teams.Update(team);
           
        }
        else
        {
            Team newTeam = new Team { Name = teamName };
            newTeam.Users.Add(new User { Name = name });
           await context.Teams.AddAsync(newTeam);
          
        }
      await  context.SaveChangesAsync();
        await Clients.Group(teamName).SendAsync("ReceiveMessageByGroup",name,team.Id);

    }


    //Grouplarda bulunan daha önceki hala kayıtlı herkesi ekrana getirme
    public async Task GetNamesByGroup()
    {
        var teams = context.Teams.Include(s => s.Users).Select(s => new
        {
            teamId = s.Id,
            Users = s.Users.ToList()
        });
        await Clients.All.SendAsync("ReceiveNamesByGroup",teams);
    }


    public override async  Task OnConnectedAsync()
    {
        ClientCount ++;
        await Clients.All.SendAsync("ReceiveClientCount",ClientCount);
        await base.OnConnectedAsync();
    }
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        ClientCount --;
        await Clients.All.SendAsync("ReceiveCleintCount",ClientCount);
        await base.OnDisconnectedAsync(exception);
    }
}
