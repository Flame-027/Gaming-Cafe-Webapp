namespace Bookings.Dtos
{
    public class GetBlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "Untitled Blog";
        public string Content { get; set; } = "Empty Blog Content";
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}