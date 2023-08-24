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
            CreateMap<Message,MessagesDtO>()
                .ForMember(d => d.SenderPhotoUrl , o => o.MapFrom(s => s.Sender.Photos
                .FirstOrDefault(x => x.IsMain).Url) )
                .ForMember(d => d.RecipientPhotoUrl , o => o.MapFrom(s => s.Recipient.Photos
                .FirstOrDefault(x => x.IsMain).Url));
            CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ?
            DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d,DateTimeKind.Utc));
        }

    }
}
