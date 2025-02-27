using AutoMapper;
using DTOs.Salon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utils;
using DA.Entities;
using DTOs.ServiceStylist;

namespace DTOs.Mappers
{
    public class SalonMapper : BaseMapper
    {
        
        public SalonMapper(IMapper mapper) : base(mapper)
        {

        }
        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<DA.Entities.Salon, SalonItemDTO>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.County.Name));

            config.CreateMap<SalonItemDTO, DA.Entities.Salon>();

            config.CreateMap<DA.Entities.Salon, CreateSalonDTO>()
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(dest => dest.CountyId, opt => opt.MapFrom(src => src.CountyId));

            config.CreateMap<CreateSalonDTO, DA.Entities.Salon>()
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => 2));

            config.CreateMap<UpdatedSalonDTO, DA.Entities.Salon>();

            config.CreateMap<ServiceDetailsVw, SalonDetailsDTO>();

            config.CreateMap<DA.Entities.Salon, SalonDetailsDTO>()
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
                .ForMember(dest => dest.County, opt => opt.MapFrom(src => src.County.Name))
                .ForMember(dest => dest.SalonId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.SalonName, opt => opt.MapFrom(src => src.Name));

        }
    }
    

}
