using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models;

public partial class Alertmessage
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int UserId { get; set; }

    public string Description { get; set; } = null!;
    public virtual User? User { get; set; } = null!;
}
