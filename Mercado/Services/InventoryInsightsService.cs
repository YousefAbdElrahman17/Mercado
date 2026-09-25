// Services/InventoryInsightsService.cs
namespace Mercado.Services
{
    public class InventoryInsightsService : IInventoryInsightsService
    {
        private readonly IInventoryContextBuilder _contextBuilder;
        private readonly IGeminiService _gemini;

        public InventoryInsightsService(IInventoryContextBuilder contextBuilder, IGeminiService gemini)
        {
            _contextBuilder = contextBuilder;
            _gemini = gemini;
        }

        public async Task<string> AskAnalyticsAsync(string question)
        {
            var context = await _contextBuilder.BuildContextAsync();
            return await _gemini.AskAsync(context, question);
        }

        public async Task<string> GenerateRecommendationsAsync()
        {
            var context = await _contextBuilder.BuildContextAsync();
            var prompt = "Suggest which products should be restocked based on available quantity and view count " +
                         "(a product with low stock and high views should be higher priority). Rank them by priority.";
            return await _gemini.AskAsync(context, prompt);
        }

        public async Task<string> GenerateReportAsync()
        {
            var context = await _contextBuilder.BuildContextAsync();
            var prompt = "Write a short, clear report on the current inventory status: number of products, " +
                         "total inventory value, the most popular products, and products that need attention.";
            return await _gemini.AskAsync(context, prompt);
        }
    }
}