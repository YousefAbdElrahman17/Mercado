// Models/ChatDocument.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mercado.Models
{
    public class ChatDocument
    {
        public int ChatDocumentId { get; set; }

        [Required(ErrorMessage = "File name is required")]
        [StringLength(255, ErrorMessage = "File name cannot exceed 255 characters")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "Extracted text is required")]
        public string ExtractedText { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}