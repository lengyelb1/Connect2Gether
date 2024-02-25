using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models;

public partial class UserPost
{
    public int Id { get; set; }

    public int ImageId { get; set; }

    public string Description { get; set; } = null!;

    public string Title { get; set; } = null!;

    public long Like { get; set; }

    public int UserId { get; set; }
    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
