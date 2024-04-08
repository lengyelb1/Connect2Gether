namespace Connect2Gether_API.Models.Dtos.UserDtos
{
    public class UserProfileDto
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int Points { get; set; }
        public Rank? Rank { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int PostCount { get; set; }
        public int CommentCount { get; set; }
    }
}
