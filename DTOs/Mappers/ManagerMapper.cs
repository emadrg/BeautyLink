using AutoMapper;
using Common.Enums;
using DA.Entities;
using DTOs.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DTOs.Mappers
{
    public class ManagerMapper : BaseMapper
    {
        public ManagerMapper(IMapper mapper) : base(mapper)
        {

        }
        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<ManagerDTO, DA.Entities.Manager>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));


            config.CreateMap<RegisterManagerDTO, DA.Entities.Manager>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

            config.CreateMap<RegisterManagerDTO, User>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
               .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
               .ForMember(dest => dest.LastModifiedDate, opt => opt.MapFrom(src => DateTime.Now))
               .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (int)Roles.Manager))
               .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => (byte)Common.Enums.UserStatus.Active));
        }
    }
}
