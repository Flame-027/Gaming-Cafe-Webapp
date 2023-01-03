using Bookings.Dtos;
using Bookings.Models;

namespace Bookings.Services.BlogService
{
    public interface IBlogService
    {
        Task<ServiceResponse<List<GetBlogDto>>> AddBlog(AddBlogDto newBlog, int id);
        Task<ServiceResponse<List<GetBlogDto>>> AddUserBlog(AddBlogDto newBlog);
        Task<ServiceResponse<List<GetBlogDto>>> DeleteBlog(int id);
        Task<ServiceResponse<List<GetBlogDto>>> DeleteUserBlog(int id);
        Task<ServiceResponse<List<GetBlogDto>>> GetAllBlogs();
        Task<ServiceResponse<List<GetBlogDto>>> GetUserBlogs();
        Task<ServiceResponse<List<GetBlogDto>>> GetUserBlogsById(int userId);
        Task<ServiceResponse<GetBlogDto>> GetBlogById(int id);
        Task<ServiceResponse<GetBlogDto>> UpdateBlog(UpdateBlogDto updatedBlog);
        Task<ServiceResponse<GetBlogDto>> UpdateCurrentBlog(UpdateBlogDto updatedBlog);
    }
}