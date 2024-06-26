﻿using System;
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

    public byte[]? ProfileImage { get; set; }

    public string? ValidatedKey { get; set; }

    [JsonIgnore]
    public virtual ICollection<Alertmessage>? Alertmessages { get; set; } = new List<Alertmessage>();

    [JsonIgnore]
    public virtual ICollection<Comment>? Comments { get; set; } = new List<Comment>();

    [JsonIgnore]
    public virtual ICollection<Deletedlike>? Deletedlikes { get; set; } = new List<Deletedlike>();

    [JsonIgnore]
    public virtual ICollection<DislikedPost>? DislikedPosts { get; set; } = new List<DislikedPost>();

    [JsonIgnore]
    public virtual ICollection<LikedPost>? LikedPosts { get; set; } = new List<LikedPost>();

    public virtual Permission Permission { get; set; } = null!;

    public virtual Rank Rank { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<UserPost>? UserPosts { get; set; } = new List<UserPost>();

    [JsonIgnore]
    public virtual UserSuspiciou? UserSuspiciou { get; set; }

    [JsonIgnore]
    public virtual ICollection<UserToken>? UserTokens { get; set; } = new List<UserToken>();
}
