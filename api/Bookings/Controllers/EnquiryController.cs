using Bookings.Dtos;
using Bookings.Models;
using Bookings.Services.EnquiryService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bookings.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]")]
    public class EnquiryController : ControllerBase
    {
        private readonly IEnquiryService _enquiryService;
        public EnquiryController(IEnquiryService enquiryService)
        {
            _enquiryService = enquiryService;
        }

        [HttpGet("GetAllEnquirys - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<List<GetEnquiryDto>>>> GetAllEnquirys()
        {
            return Ok(await _enquiryService.GetAllEnquirys());
        }

        [HttpGet("GetEnquiryById {id} - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<GetEnquiryDto>>> GetEnquiryById(int id)
        {
            var response = await _enquiryService.GetEnquiryById(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPut("UpdateEnquiry - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<GetEnquiryDto>>> UpdateEnquiry(UpdateEnquiryDto updatedEnquiry)
        {
            var response = await _enquiryService.UpdateEnquiry(updatedEnquiry);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpDelete("DeleteEnquiry {id} - ADMIN FEATURE")]
        public async Task<ActionResult<ServiceResponse<List<GetEnquiryDto>>>> DeleteEnquiry(int id)
        {
            var response = await _enquiryService.DeleteEnquiry(id);
            if (response == null)
            {
                return NotFound(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [AllowAnonymous]
        [HttpPost("AddEnquiry")]
        public async Task<ActionResult<ServiceResponse<GetEnquiryDto>>> AddEnquiry(AddEnquiryDto newEnquiry)
        {
            return Ok(await _enquiryService.AddEnquiry(newEnquiry));
        }
    }
}