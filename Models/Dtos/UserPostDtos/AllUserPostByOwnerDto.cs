using Connect2Gether_API.Models.Dtos.CommentDtos;
using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models.Dtos.UserPostDtos
{
    public class AllUserPostByOwnerDto
    {
        public int Id { get; set; }

        public int? ImageId { get; set; }

        public string Description { get; set; } = null!;

        public string Title { get; set; } = null!;

        public long Like { get; set; }

        [JsonIgnore]
        public int? UserId { get; set; }

        public bool OwnPost { get; set; }

        public string? UserName { get; set; }

        public DateTime? UploadDate { get; set; }

        public virtual ICollection<CommentResponseDto> Comments { get; set; } = new List<CommentResponseDto>();

        [JsonIgnore]
        public virtual Image? Image { get; set; }

        [JsonIgnore]
        public virtual ICollection<LikedPost>? LikedPosts { get; set; } = new List<LikedPost>();

        [JsonIgnore]
        public virtual User? User { get; set; }
    }
}
