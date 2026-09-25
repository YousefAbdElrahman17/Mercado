// Services/PdfTextExtractor.cs
using UglyToad.PdfPig;
using System.Text;

namespace Mercado.Services
{
    public class PdfTextExtractor : IPdfTextExtractor
    {
        public string ExtractText(Stream pdfStream)
        {
            using var document = PdfDocument.Open(pdfStream);
            var sb = new StringBuilder();
            foreach (var page in document.GetPages())
                sb.AppendLine(page.Text);
            return sb.ToString();
        }
    }
}