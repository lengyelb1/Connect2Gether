namespace Connect2Gether_API.Models.Dtos.UserPostDtos
{
    public class DislikedPostDto
    {
        public int postId { get; set; }
        public int userId { get; set; }
        public bool isDisliked { get; set; }
    }
}
