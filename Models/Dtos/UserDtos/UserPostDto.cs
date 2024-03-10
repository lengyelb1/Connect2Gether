using Org.BouncyCastle.Asn1.X509;

namespace Connect2Gether_API.Models.Dtos.UserDtos
{
    public class UserPostDto
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
    }
}
