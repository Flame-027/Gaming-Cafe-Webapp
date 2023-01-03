using Bookings.Dtos;
using Bookings.Models;

namespace Bookings.Services.EnquiryService
{
    public interface IEnquiryService
    {
        Task<ServiceResponse<List<GetEnquiryDto>>> DeleteEnquiry(int id);
        Task<ServiceResponse<List<GetEnquiryDto>>> GetAllEnquirys();
        Task<ServiceResponse<GetEnquiryDto>> AddEnquiry(AddEnquiryDto newEnquiry);
        Task<ServiceResponse<GetEnquiryDto>> GetEnquiryById(int id);
        Task<ServiceResponse<GetEnquiryDto>> UpdateEnquiry(UpdateEnquiryDto updatedEnquiry);
    }
}