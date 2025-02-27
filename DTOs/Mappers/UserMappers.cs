using AutoMapper;
using Common.Enums;
using DA.Entities;
using DTOs.Manager;
using DTOs.Users;
using System.Security.Claims;
using Utils;
using UserStatus = Common.Enums.UserStatus;

namespace DTOs.Mappers
{
    public class UserMappers: BaseMapper
    {
        public readonly ClaimsPrincipal CurrentUser;

        public UserMappers(IMapper mapper, ClaimsPrincipal currentUser) : base(mapper) 
        {
            CurrentUser = currentUser;
        }
        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<RegisterUserDTO, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (int)UserStatus.Active))
                .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
            
            config.CreateMap<UpdateUserDTO, User>()
                .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => DateTime.Now));

            config.CreateMap<User, UserDetailsDTO>()
             .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture != null ? src.ProfilePicture.Path : null));

            config.CreateMap<User, CurrentUserDTO>();

            config.CreateMap<User, UserListItemDTO>();

            config.CreateMap<User, DispalyUserDTO>()
                .ForMember(dest => dest.GuidId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
