using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models;

public partial class LikedPost
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PostId { get; set; }

    [JsonIgnore]
    public virtual UserPost? Post { get; set; } = null!;

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
