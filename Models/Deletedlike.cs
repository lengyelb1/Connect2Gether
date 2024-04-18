using System;
using System.Collections.Generic;

namespace Connect2Gether_API.Models;

public partial class Deletedlike
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int PostId { get; set; }

    public virtual UserPost Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
