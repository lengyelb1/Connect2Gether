﻿namespace Connect2Gether_API.Models.Dtos
{
    public class RegistrationRequestDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public RegistrationRequestDto(string? userName, string? password, string? email)
        {
            UserName = userName;
            Password = password;
            Email = email;
        }
    }
}
