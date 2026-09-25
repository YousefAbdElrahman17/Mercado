// Services/IPdfTextExtractor.cs
namespace Mercado.Services
{
    public interface IPdfTextExtractor
    {
        string ExtractText(Stream pdfStream);
    }
}