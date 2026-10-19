using ApniDukaan.Core.Entities;
using ApniDukaan.Core.ResponseDTO;
using AutoMapper;

namespace ApniDukaan.Core.Mappers
{
    public class ApplicationUserToAuthenticationResponseMappingProfile : Profile
    {
        public ApplicationUserToAuthenticationResponseMappingProfile()
        {
            // Mapping source <ApplicationUser> with destination <AuthenticationResponse>. Ignoring the fields which are not in source but present in destination.
            CreateMap<ApplicationUser, AuthenticationResponse>()
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PersonName, opt => opt.MapFrom(src => src.PersonName))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.AccessToken, opt => opt.Ignore())
                .ForMember(dest => dest.RefreshToken, opt => opt.Ignore());
        }
    }
}
