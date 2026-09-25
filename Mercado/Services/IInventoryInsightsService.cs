// Services/IInventoryInsightsService.cs
namespace Mercado.Services
{
    public interface IInventoryInsightsService
    {
        Task<string> AskAnalyticsAsync(string question);
        Task<string> GenerateRecommendationsAsync();
        Task<string> GenerateReportAsync();
    }
}