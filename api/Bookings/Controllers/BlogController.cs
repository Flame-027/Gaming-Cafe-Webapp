using System.Security.Claims;
using Bookings.Dtos;
using Bookings.Models;
using Bookings.Services.BlogService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookings.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BlogController(IBlogService blogService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _blogService = blogService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AddBlog {id} - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<List<GetBlogDto>>>> AddBlog(AddBlogDto newBlog, int id)
        {
            return Ok(await _blogService.AddBlog(newBlog, id));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateBlog - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<GetBlogDto>>> UpdateBlog(UpdateBlogDto updatedBlog)
        {
            var response = await _blogService.UpdateBlog(updatedBlog);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteBlog {id} - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<List<GetBlogDto>>>> DeleteBlog(int id)
        {
            var response = await _blogService.DeleteBlog(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [Authorize]
        [HttpPost("AddBlogToCurrentUser")]
        public async Task<ActionResult<ServiceResponse<List<GetBlogDto>>>> AddUserBlog(AddBlogDto newBlog)
        {
            return Ok(await _blogService.AddUserBlog(newBlog));
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetBlogDto>>>> Get()
        {
            return Ok(await _blogService.GetAllBlogs());
        }

        [Authorize]
        [HttpGet("GetAllCurrentUserBlogs")]
        // Essentially the same as GetAllUserBlogsByUserId {id} because all blogs are public, except the user id does not have to be inputted
        public async Task<ActionResult<ServiceResponse<List<GetBlogDto>>>> GetUserBlogs()
        {
            return Ok(await _blogService.GetUserBlogs());
        }

        [HttpGet("GetAllUserBlogsByUserId {id}")]
        public async Task<ActionResult<ServiceResponse<List<GetBlogDto>>>> GetUserBlogsById(int id)
        {
            return Ok(await _blogService.GetUserBlogsById(id));
        }

        [HttpGet("GetBlogById {id}")]
        public async Task<ActionResult<ServiceResponse<GetBlogDto>>> GetBlogById(int id)
        {
            var response = await _blogService.GetBlogById(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [Authorize]
        [HttpPut("UpdateCurrentBlog")]
        public async Task<ActionResult<ServiceResponse<GetBlogDto>>> UpdateCurrentBlog(UpdateBlogDto updatedBlog)
        {
            var response = await _blogService.UpdateCurrentBlog(updatedBlog);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [Authorize]
        [HttpDelete("DeleteUserBlog {id}")]
        public async Task<ActionResult<ServiceResponse<List<GetBlogDto>>>> DeleteUserBlog(int id)
        {
            var response = await _blogService.DeleteUserBlog(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }
    }
}