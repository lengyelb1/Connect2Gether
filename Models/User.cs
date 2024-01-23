using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Hash { get; set; } = null!;

    public string Email { get; set; } = null!;

    public bool ActiveUser { get; set; }

    /// <summary>
    /// Pontszámhoz kötött rangok
    /// </summary>
    public int RankId { get; set; }

    public DateTime RegistrationDate { get; set; }

    /// <summary>
    /// Pontszám
    /// </summary>
    public int Point { get; set; }

    /// <summary>
    /// Felhasználói szint
    /// </summary>
    public int PermissionId { get; set; }

    public DateTime LastLogin { get; set; }

    public virtual ICollection<Alertmessage>? Alertmessages { get; set; } = new List<Alertmessage>();

    public virtual ICollection<Comment>? Comments { get; set; } = new List<Comment>();

    public virtual Permission Permission { get; set; } = null!;

    public virtual ICollection<UserPost> UserPosts { get; set; } = new List<UserPost>();
}
