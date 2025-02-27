using AutoMapper;
using DTOs.ServiceStylist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DTOs.Mappers
{
    public class ServiceStylistMapper : BaseMapper
    {
        public ServiceStylistMapper(IMapper mapper) : base(mapper)
        {

        }

        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<DA.Entities.ServiceStylist, ServiceStylistDTO>()
                .ForMember(dest => dest.Service, opt => opt.MapFrom(src => src.Service.Name))
                .ForMember(dest => dest.Stylist, opt => opt.MapFrom(src => $"{src.Stylist.User.FirstName} {src.Stylist.User.LastName}"));

            config.CreateMap<EditOrAddServiceStylistDTO, DA.Entities.ServiceStylist>();
             
        }
    }
}
