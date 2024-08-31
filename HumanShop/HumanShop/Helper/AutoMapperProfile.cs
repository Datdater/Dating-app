using AutoMapper;
using HumanShop.DTOs;
using HumanShop.Entities;

namespace HumanShop.NewFolder
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AppUser, MemberDTO>()
                .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain)!.URL));
            CreateMap<Photo, PhotoDTO>();
        }
    }
}
