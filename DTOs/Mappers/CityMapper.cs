using AutoMapper;
using DTOs.City;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DTOs.Mappers
{
    public class CityMapper : BaseMapper
    {
        public CityMapper(IMapper mapper) : base(mapper)
        {

        }
        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<DA.Entities.City, CityDTO>();

        }
    }
}
