using AutoMapper;
using DTOs.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DTOs.Mappers
{
    public class ServiceEntityMapper : BaseMapper
    {
        public ServiceEntityMapper(IMapper mapper) : base(mapper)
        {

        }

        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<DA.Entities.Service, ServiceDTO>();
        }
    }
}
