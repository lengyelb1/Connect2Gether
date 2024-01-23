using System;
using System.Collections.Generic;

namespace Connect2Gether_API.Models;

public partial class Comment
{
    public int Id { get; set; }

    public string Text { get; set; } = null!;

    public int PostId { get; set; }

    public int UserId { get; set; }

    public int CommentId { get; set; }

    public virtual UserPost Post { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
