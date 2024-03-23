using SignalR.WebAPI.Models;

namespace SignalR.WebAPI.Hubs;

public interface IProductHub
{
    Task ReceiveProduct(Product product);
}
