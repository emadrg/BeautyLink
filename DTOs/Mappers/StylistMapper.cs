using AutoMapper;
using Common.Enums;
using DA.Entities;
using DTOs.ServiceStylist;
using DTOs.Stylist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DTOs.Mappers
{
    public class StylistMapper : BaseMapper
    {
        public StylistMapper(IMapper mapper) : base(mapper)
        {

        }

        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<DA.Entities.Stylist, StylistDTO>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.ProfilePictureId, opt => opt.MapFrom(src => src.User.ProfilePictureId))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Salon, opt => opt.MapFrom(src => src.Salon.Name))
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.User.ProfilePicture != null ? src.User.ProfilePicture.Path : null))
                .ForMember(dest => dest.AverageScore, opt => opt.Ignore());

            config.CreateMap<RegisterStylistDTO, User>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
               .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
               .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (int)Roles.Stylist))
               .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());

            config.CreateMap<RegisterStylistDTO, DA.Entities.Stylist>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            config.CreateMap<DA.Entities.Stylist, StylistWithServicesDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
                .ForMember(dest => dest.ProfilePictureId, opt => opt.MapFrom(src => src.User.ProfilePictureId))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.User.PhoneNumber))
                .ForMember(dest => dest.Services, opt => opt.Ignore());

        }
    }
}
