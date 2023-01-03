using System.Security.Claims;
using AutoMapper;
using Bookings.Data;
using Bookings.Dtos;
using Bookings.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Services.BlogService
{
    public class BlogService : IBlogService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BlogService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        public async Task<ServiceResponse<List<GetBlogDto>>> AddBlog(AddBlogDto newBlog, int id)
        {
            var response = new ServiceResponse<List<GetBlogDto>>();
            Blog blog = _mapper.Map<Blog>(newBlog);

            // Assign found id of User to Blog.UserId
            var dbUser = await _context.Users.FirstOrDefaultAsync(c => c.Id == id);
            if (dbUser != null)
            {
                blog.UserId = dbUser.Id;
            }
            else
            {
                response.Success = false;
                response.Message = "Unable to find User with that Id.";
                return response;
            }

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            response.Data = await _context.Blogs.Select(c => _mapper.Map<GetBlogDto>(c)).ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<List<GetBlogDto>>> AddUserBlog(AddBlogDto newBlog)
        {
            var response = new ServiceResponse<List<GetBlogDto>>();
            Blog blog = _mapper.Map<Blog>(newBlog);

            blog.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            _context.Blogs.Add(blog);
            await _context.SaveChangesAsync();
            response.Data = await _context.Blogs
                .Where(c => c.User.Id == GetUserId())
                .Select(c => _mapper.Map<GetBlogDto>(c)).ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<List<GetBlogDto>>> DeleteBlog(int id)
        {
            var response = new ServiceResponse<List<GetBlogDto>>();
            try
            {
                Blog blog = await _context.Blogs.FirstAsync(c => c.Id == id);
                if (blog != null)
                {
                    _context.Blogs.Remove(blog);
                    await _context.SaveChangesAsync();
                    response.Data = _context.Blogs.Select(c => _mapper.Map<GetBlogDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Blog not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetBlogDto>>> DeleteUserBlog(int id)
        {
            var response = new ServiceResponse<List<GetBlogDto>>();
            try
            {
                Blog blog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
                if (blog != null)
                {
                    _context.Blogs.Remove(blog);
                    await _context.SaveChangesAsync();
                    response.Data = _context.Blogs
                        .Where(c => c.User.Id == GetUserId())
                        .Select(c => _mapper.Map<GetBlogDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Blog not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetBlogDto>>> GetAllBlogs()
        {
            var response = new ServiceResponse<List<GetBlogDto>>();
            var dbBlogs = await _context.Blogs.ToListAsync();
            response.Data = dbBlogs.Select(c => _mapper.Map<GetBlogDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<List<GetBlogDto>>> GetUserBlogs()
        {
            var response = new ServiceResponse<List<GetBlogDto>>();
            var dbBlogs = await _context.Blogs.Where(c => c.User.Id == GetUserId()).ToListAsync();
            response.Data = dbBlogs.Select(c => _mapper.Map<GetBlogDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<List<GetBlogDto>>> GetUserBlogsById(int userId)
        {
            var response = new ServiceResponse<List<GetBlogDto>>();
            var dbBlogs = await _context.Blogs.Where(c => c.User.Id == userId).ToListAsync();
            response.Data = dbBlogs.Select(c => _mapper.Map<GetBlogDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetBlogDto>> GetBlogById(int id)
        {
            var response = new ServiceResponse<GetBlogDto>();
            var dbBlog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == id);
            response.Data = _mapper.Map<GetBlogDto>(dbBlog);
            return response;
        }

        public async Task<ServiceResponse<GetBlogDto>> UpdateBlog(UpdateBlogDto updatedBlog)
        {
            var response = new ServiceResponse<GetBlogDto>();
            try
            {
                Blog blog = await _context.Blogs.FirstOrDefaultAsync(c => c.Id == updatedBlog.Id);

                blog.Title = updatedBlog.Title;
                blog.Content = updatedBlog.Content;
                blog.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetBlogDto>(blog);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<GetBlogDto>> UpdateCurrentBlog(UpdateBlogDto updatedBlog)
        {
            var response = new ServiceResponse<GetBlogDto>();
            try
            {
                Blog blog = await _context.Blogs
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == updatedBlog.Id);
                if (blog.User.Id == GetUserId())
                {
                    blog.Title = updatedBlog.Title;
                    blog.Content = updatedBlog.Content;
                    blog.UpdatedDate = DateTime.Now;

                    await _context.SaveChangesAsync();

                    response.Data = _mapper.Map<GetBlogDto>(blog);
                }
                else
                {
                    response.Success = false;
                    response.Message = "Blog not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}