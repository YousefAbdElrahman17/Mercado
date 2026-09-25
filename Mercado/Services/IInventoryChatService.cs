// Services/IInventoryChatService.cs
namespace Mercado.Services
{
    public interface IInventoryChatService
    {
        Task<string> AskAboutInventoryAsync(string question);
    }
}