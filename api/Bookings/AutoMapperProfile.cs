using AutoMapper;
using Bookings.Dtos;
using Bookings.Models;

namespace Bookings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, GetUserDto>();
            CreateMap<Booking, GetBookingDto>();
            CreateMap<AddBookingDto, Booking>();
            CreateMap<Blog, GetBlogDto>();
            CreateMap<AddBlogDto, Blog>();
            CreateMap<Enquiry, GetEnquiryDto>();
            CreateMap<AddEnquiryDto, Enquiry>();
            CreateMap<GameEvent, GetGameEventDto>();
            CreateMap<AddGameEventDto, GameEvent>();
        }
    }
}