using AutoMapper;
using Domains.Entities;
using Domains.Identity;
using Domains.Views;
using Shared.DTO.Entities;
using Shared.DTO.Views;
using Shared.DTOs.User;

namespace BL.Mapper
{
    // Main mapping profile file (MappingProfile.cs)
    public partial class MappingProfile : Profile
    {
        public MappingProfile()
        {
           
            CreateMap<TbAuthor, AuthorDto>().ReverseMap();
            CreateMap<TbCategory, CategoryDto>().ReverseMap();
            CreateMap<TbFavoriteBook, FavoriteBookDto>().ReverseMap();
            CreateMap<TbBook, BookDto>().ReverseMap();
            CreateMap<TbSettings, SettingDto>().ReverseMap();
            CreateMap<TbReview, ReviewDto>().ReverseMap();
            CreateMap<TbReport,ReportDto>().ReverseMap();

            
            CreateMap<TbRefreshToken, RefreshTokenDto>().ReverseMap();
            CreateMap<ApplicationUser, UserRegistrationDto>().ReverseMap();
            CreateMap<VwBook, BookViewDto>().ReverseMap();
            CreateMap<VwUserProfile, UserProfileViewDto>().ReverseMap();




        }
    }
}
