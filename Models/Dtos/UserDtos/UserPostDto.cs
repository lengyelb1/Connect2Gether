using Org.BouncyCastle.Asn1.X509;
using System.Numerics;
using System.Text.Json.Serialization;

namespace Connect2Gether_API.Models.Dtos.UserDtos
{
    public class UserPostDto
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public int UserId { get; set; }
        public Image? Image { get; set; } = null;
    }
}
