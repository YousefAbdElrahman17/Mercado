// Services/IProductViewService.cs
namespace Mercado.Services
{
    public interface IProductViewService
    {
        Task IncrementViewCountAsync(int productId);
    }
}