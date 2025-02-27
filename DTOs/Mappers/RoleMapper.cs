using AutoMapper;
using DTOs.Manager;
using DTOs.Role;
using DA.Entities;
using System;   
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;
using DTOs.Salon;

namespace DTOs.Mappers
{
    public class RoleMapper : BaseMapper
    {
        public RoleMapper(IMapper mapper) : base(mapper)
        {

        }

        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<DA.Entities.Role, RoleDTO>();

        }
    }
}
