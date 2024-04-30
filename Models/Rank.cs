using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models;

public partial class Rank
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int MinPont { get; set; }

    public int MaxPont { get; set; }

    public string Description { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<User>? Users { get; set; } = new List<User>();
}
