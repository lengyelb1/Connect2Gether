using System;
using System.Collections.Generic;

namespace Connect2Gether_API.Models;

public partial class UserSuspiciou
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
