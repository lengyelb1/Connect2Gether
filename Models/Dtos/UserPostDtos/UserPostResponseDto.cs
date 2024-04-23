using Connect2Gether_API.Models.Dtos.CommentDtos;
using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models.Dtos.UserPostDtos
{
    public class UserPostResponseDto
    {
        public int Id { get; set; }
        public byte[]? Image { get; set; }
        public string Description { get; set; } = null!;
        public string Title { get; set; } = null!;
        public long Like { get; set; }
        public bool Liked { get; set; }
        public bool Disliked { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime? UploadDate { get; set; }
        public virtual ICollection<CommentResponseDto> Comments { get; set; } = new List<CommentResponseDto>();

        [JsonIgnore]
        public virtual User? User { get; set; }

    }
}
