using AutoMapper;
using Bookings.Data;
using Bookings.Dtos;
using Bookings.Models;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Services.EnquiryService
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        public EnquiryService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetEnquiryDto>> AddEnquiry(AddEnquiryDto newEnquiry)
        {
            var response = new ServiceResponse<GetEnquiryDto>();
            Enquiry enquiry = _mapper.Map<Enquiry>(newEnquiry);

            _context.Enquirys.Add(enquiry);
            await _context.SaveChangesAsync();

            var dbEnquiry = await _context.Enquirys.FirstOrDefaultAsync(c => c.Id == enquiry.Id);
            response.Data = _mapper.Map<GetEnquiryDto>(dbEnquiry);
            return response;
        }

        public async Task<ServiceResponse<List<GetEnquiryDto>>> DeleteEnquiry(int id)
        {
            var response = new ServiceResponse<List<GetEnquiryDto>>();
            try
            {
                Enquiry enquiry = await _context.Enquirys.FirstAsync(c => c.Id == id);
                if (enquiry != null)
                {
                    _context.Enquirys.Remove(enquiry);
                    await _context.SaveChangesAsync();
                    response.Data = _context.Enquirys.Select(c => _mapper.Map<GetEnquiryDto>(c)).ToList();
                }
                else
                {
                    response.Success = false;
                    response.Message = "Enquiry not found.";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetEnquiryDto>>> GetAllEnquirys()
        {
            var response = new ServiceResponse<List<GetEnquiryDto>>();
            var dbEnquirys = await _context.Enquirys.ToListAsync();
            response.Data = dbEnquirys.Select(c => _mapper.Map<GetEnquiryDto>(c)).ToList();
            return response;
        }

        public async Task<ServiceResponse<GetEnquiryDto>> GetEnquiryById(int id)
        {
            var response = new ServiceResponse<GetEnquiryDto>();
            var dbEnquiry = await _context.Enquirys.FirstOrDefaultAsync(c => c.Id == id);
            response.Data = _mapper.Map<GetEnquiryDto>(dbEnquiry);
            return response;
        }

        public async Task<ServiceResponse<GetEnquiryDto>> UpdateEnquiry(UpdateEnquiryDto updatedEnquiry)
        {
            var response = new ServiceResponse<GetEnquiryDto>();
            try
            {
                Enquiry enquiry = await _context.Enquirys.FirstOrDefaultAsync(c => c.Id == updatedEnquiry.Id);

                enquiry.Name = updatedEnquiry.Name;
                enquiry.Email = updatedEnquiry.Email;
                enquiry.EnquiryText = updatedEnquiry.EnquiryText;
                enquiry.TicketResolved = updatedEnquiry.TicketResolved;

                await _context.SaveChangesAsync();

                response.Data = _mapper.Map<GetEnquiryDto>(enquiry);
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