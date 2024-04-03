namespace Connect2Gether_API.Models.Dtos
{
    public class RegistrationRequestDto
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public bool ActiveUser { get; set; }
        public int RankId { get; set; }
    }
}
