using System;
using System.Collections.Generic;

namespace Connect2Gether_API.Models;

public partial class UserToken
{
    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime TokenExpireDate { get; set; }

    public virtual User User { get; set; } = null!;
}
