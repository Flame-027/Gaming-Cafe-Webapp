namespace Bookings.Dtos
{
    public class UpdateBlogDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = "Untitled Blog";
        public string Content { get; set; } = "Empty Blog Content";
    }
}