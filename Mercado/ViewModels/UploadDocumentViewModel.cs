using System.ComponentModel.DataAnnotations;

namespace Mercado.ViewModels
{
    public class UploadDocumentViewModel
    {
        [Required(ErrorMessage = "Please select a PDF file first")]
        public IFormFile File { get; set; }
    }
}