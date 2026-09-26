using Microsoft.AspNetCore.Mvc;
using Mercado.Filters;
using Mercado.Services;
using Mercado.ViewModels;

namespace Mercado.Controllers
{
    [RequireLogin]
    public class InsightsController : Controller
    {
        private readonly IDocumentChatService _documentChat;
        private readonly IInventoryInsightsService _insights;

        public InsightsController(IDocumentChatService documentChat, IInventoryInsightsService insights)
        {
            _documentChat = documentChat;
            _insights = insights;
        }

        private int CurrentUserId => HttpContext.Session.GetInt32("UserId")!.Value;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var doc = await _documentChat.GetActiveDocumentAsync(CurrentUserId);

            var vm = new InsightsDashboardViewModel
            {
                ActiveDocumentName = doc?.FileName,
                ActiveDocumentUploadedAt = doc?.UploadedAt
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> UploadDocument(UploadDocumentViewModel vm)
        {
            if (!ModelState.IsValid || vm.File.ContentType != "application/pdf")
            {
                TempData["Error"] = "Please upload a valid PDF file";
                return RedirectToAction(nameof(Index));
            }

            await _documentChat.UploadDocumentAsync(vm.File, CurrentUserId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDocument()
        {
            await _documentChat.DeleteActiveDocumentAsync(CurrentUserId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> AskDocument([FromBody] DocumentChatRequestViewModel request)
        {
            var reply = await _documentChat.AskAsync(CurrentUserId, request.Question);
            return Json(new DocumentChatResponseViewModel { Reply = reply });
        }

        [HttpPost]
        public async Task<IActionResult> AskAnalytics([FromBody] AnalyticsRequestViewModel request)
        {
            var reply = await _insights.AskAnalyticsAsync(request.Question);
            return Json(new AnalyticsResponseViewModel { Reply = reply });
        }

        [HttpPost]
        public async Task<IActionResult> GetRecommendations()
        {
            var reply = await _insights.GenerateRecommendationsAsync();
            return Json(new RecommendationsResponseViewModel { Reply = reply });
        }

        [HttpPost]
        public async Task<IActionResult> GetReport()
        {
            var reply = await _insights.GenerateReportAsync();
            return Json(new ReportResponseViewModel { Reply = reply });
        }
    }
}