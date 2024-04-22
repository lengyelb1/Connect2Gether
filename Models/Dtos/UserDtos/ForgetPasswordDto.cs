namespace Connect2Gether_API.Models.Dtos.UserDtos
{
    public class ForgetPasswordDto
    {
        public string? UserName { get; set; }
        public string? NewPassword { get; set; }

        public string? NewPasswordAgain { get; set; }
    }
}
