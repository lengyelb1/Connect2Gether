namespace Connect2Gether_API.Models.Dtos
{
    public class RegistrationRequestDto
    {
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public RegistrationRequestDto(int userId ,string? userName, string? password, string? email)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            Email = email;
        }
    }
}
