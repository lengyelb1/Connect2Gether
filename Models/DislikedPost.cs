using System;
using System.Collections.Generic;

namespace Connect2Gether_API.Models;

public partial class DislikedPost
{
    public int Id { get; set; }

    public int Userid { get; set; }

    public int Postid { get; set; }

    public virtual UserPost Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
