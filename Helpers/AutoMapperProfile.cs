using AutoMapper;
using datingAppreal.DTOs;
using datingAppreal.Entities;
using datingAppreal.Extensions;

namespace datingAppreal.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
           
            CreateMap<User, MemberDtO>()
             .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                 src.Photos.FirstOrDefault(x => x.IsMain).Url))
             .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberUpdateDtO, User>();
            CreateMap<RegisterDtO, User>(); 

        }

    }
}
