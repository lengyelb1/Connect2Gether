﻿namespace Connect2Gether_API.Models.Dtos.UserDtos
{
    public class AdminUserPutDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int permissionId { get; set; }
    }
}
