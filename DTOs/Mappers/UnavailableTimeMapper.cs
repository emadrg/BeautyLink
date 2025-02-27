using AutoMapper;
using DTOs.Schedule;
using DTOs.StylistUnavailableTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DTOs.Mappers
{
    public class UnavailableTimeMapper : BaseMapper
    {
        public UnavailableTimeMapper(IMapper mapper) : base(mapper)
        {
            
        }

        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<DA.Entities.UnavailableTime, UnavailableTimeDTO>();

            config.CreateMap<CreateUnavailableTimeDTO, DA.Entities.UnavailableTime>()
                .ForMember(dest => dest.StylistId, opt => opt.Ignore());

        }
    }
}
