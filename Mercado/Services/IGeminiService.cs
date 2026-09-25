// Services/IGeminiService.cs
namespace Mercado.Services
{
    public interface IGeminiService
    {
        Task<string> AskAsync(string context, string question);
    }
}