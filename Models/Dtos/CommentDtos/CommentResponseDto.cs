using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models.Dtos.CommentDtos
{
    public class CommentResponseDto
    {
        public int Id { get; set; }

        public string Text { get; set; } = null!;

        public int PostId { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public int CommentId { get; set; }

        public DateTime? UploadDate { get; set; }
    }
}
