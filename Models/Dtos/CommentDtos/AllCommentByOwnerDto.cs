using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models.Dtos.CommentDtos
{
    public class AllCommentByOwnerDto
    {
        public int Id { get; set; }

        public string Text { get; set; } = null!;

        public int PostId { get; set; }

        public int UserId { get; set; }

        public int CommentId { get; set; }

        public bool OwnComment { get; set; }

        public string? UserName { get; set; }

        public DateTime? UploadDate { get; set; }

        [JsonIgnore]
        public virtual UserPost? Post { get; set; } = null!;

        [JsonIgnore]
        public virtual User? User { get; set; } = null!;
    }
}
