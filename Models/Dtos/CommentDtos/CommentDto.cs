namespace Connect2Gether_API.Models.Dtos.CommentDtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
    }
}
