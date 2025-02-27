using AutoMapper;
using DTOs.County;
using DTOs.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DTOs.Mappers
{
    public class ScheduleMapper : BaseMapper
    {
        public ScheduleMapper(IMapper mapper) : base(mapper)
        {
            
        }
        public override void Config(IMapperConfigurationExpression config)
        {
            config.CreateMap<DA.Entities.Schedule, ScheduleDTO>();

            config.CreateMap<DA.Entities.Schedule, CreateOrUpdateScheduleDTO>();

            config.CreateMap<CreateOrUpdateScheduleDTO, DA.Entities.Schedule>();

            config.CreateMap<UpdateScheduleDTO, DA.Entities.Schedule>();

            config.CreateMap<DA.Entities.WeekDay, WeekdayDTO>();

        }

    }
}
