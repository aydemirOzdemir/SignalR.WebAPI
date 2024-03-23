using Microsoft.AspNetCore.SignalR;

namespace SignalR.WebAPI.Hubs;

public class ProductHub:Hub<IProductHub>
{
    public async Task SendProductAsync(Models.Product product)
    {
        await Clients.All.ReceiveProduct(product);
    }// tip güvenli bir şekilde client tarafındaki metotlarımın ismini belirtmiş oldum

}
