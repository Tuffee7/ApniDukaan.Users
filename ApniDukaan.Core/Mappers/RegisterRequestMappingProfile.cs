using ApniDukaan.Core.Entities;
using ApniDukaan.Core.RequestDTO;
using AutoMapper;

namespace ApniDukaan.Core.Mappers
{
    public class RegisterRequestMappingProfile : Profile
    {
        public RegisterRequestMappingProfile()
        {
            // Mapping source <RegisterRequest> with destination <ApplicationUser>. Ignoring the fields which are not in source but present in destination.
            CreateMap<RegisterRequest, ApplicationUser>()
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                    .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)) // Note: Password will be hashed in the service layer, not in the mapping
                    .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.PersonName))
                    .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));
        }
    }
}
