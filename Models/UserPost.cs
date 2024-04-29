using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models;

public partial class UserPost
{
    public int Id { get; set; }

    public byte[]? Image { get; set; }

    public string Description { get; set; } = null!;

    public string Title { get; set; } = null!;

    public long Like { get; set; }

    public int Dislike { get; set; }

    public int? UserId { get; set; }

    public DateTime? UploadDate { get; set; }

    public virtual ICollection<Comment>? Comments { get; set; } = new List<Comment>();

    [JsonIgnore]
    public virtual ICollection<Deletedlike>? Deletedlikes { get; set; } = new List<Deletedlike>();

    [JsonIgnore]
    public virtual ICollection<DislikedPost>? DislikedPosts { get; set; } = new List<DislikedPost>();

    [JsonIgnore]
    public virtual ICollection<LikedPost>? LikedPosts { get; set; } = new List<LikedPost>();

    public virtual User? User { get; set; }
}
