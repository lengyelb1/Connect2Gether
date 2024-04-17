﻿using Connect2Gether_API.Models.Dtos.CommentDtos;
using Connect2Gether_API.Models.Dtos.UserPostDtos;
using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models.Dtos.UserDtos
{
    public class UserByIdDto
    {
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public DateTime RegistrationDate { get; set; }

        public DateTime LastLogin { get; set; }

        public virtual ICollection<UserPostResponseDto>? UserPosts { get; set; } = new List<UserPostResponseDto>();
    }
}
