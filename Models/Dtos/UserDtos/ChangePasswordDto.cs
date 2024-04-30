namespace Connect2Gether_API.Models.Dtos.UserDtos
{
    public class ChangePasswordDto
    {

        public string? OldPassword { get; set; }

        public string? NewPassword { get; set; }

        public string? NewPasswordAgain { get; set; }

    }
}
