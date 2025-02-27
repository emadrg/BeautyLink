using AutoMapper;
using DTOs.County;
using DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DTOs.Mappers
{
    public class CountyMapper : BaseMapper
    {
        public CountyMapper(IMapper mapper) : base(mapper)
        {

        }
        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<DA.Entities.County, CountyDTO>();

        }
    }
}
