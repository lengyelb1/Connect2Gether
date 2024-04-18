namespace Connect2Gether_API.Models.Dtos.UserPostDtos
{
    public class LikedPostDto
    {
        public int postId { get; set; }
        public int userId { get; set; }
        public bool isLiked { get; set; }
        public bool rewardClaimed { get; set; }

    }
}
