using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models.Dtos.UserPostDtos
{
    public class UserPostDtoToLike
    {
        public int Id { get; set; }

        public byte[]? Image { get; set; }

        public string Description { get; set; } = null!;

        public string Title { get; set; } = null!;

        public long Like { get; set; }

        [JsonIgnore]
        public int? UserId { get; set; }

        public bool Liked { get; set; }

        public DateTime? UploadDate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        [JsonIgnore]
        public virtual ICollection<LikedPost>? LikedPosts { get; set; } = new List<LikedPost>();

        public virtual User? User { get; set; }
    }
}
