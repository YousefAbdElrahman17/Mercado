// Services/IInventoryContextBuilder.cs
namespace Mercado.Services
{
    public interface IInventoryContextBuilder
    {
        Task<string> BuildContextAsync();
    }
}