using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
using DTOs.Mappers;
using DTOs.Schedule;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace BL.Services
{
    public class ScheduleService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public ScheduleService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<List<ScheduleDTO>> GetScheduleByStylistId (Guid stylistId)
        {
            var schedule =  await UnitOfWork.Queryable<Schedule>()
                .Where(e => e.StylistId == stylistId).ToListAsync();

            return Mapper.Map<Schedule, ScheduleDTO>(schedule);
        }

        public async Task<List<WeekdayDTO>> GetWeekdays ()
        {
            var weekdays = await UnitOfWork.Queryable<WeekDay>().ToListAsync();
            return Mapper.Map<WeekDay, WeekdayDTO>(weekdays); 
        }

        public async Task<int> DeleteSchedule(int weekDay, Guid stylistId)
        {
            var schedule = await UnitOfWork.Queryable<Schedule>()
                .FirstOrDefaultAsync(s => s.WeekDayId == weekDay && s.StylistId == stylistId);

            UnitOfWork.Repository<Schedule>().Remove(schedule);

            return await Save();
             
        }

        public async Task<int> CreateOrUpdate (CreateOrUpdateScheduleDTO createOrUpdateSchedule, Guid stylistId)
        {
            var schedule = await UnitOfWork.Queryable<Schedule>()
                .FirstOrDefaultAsync(s => s.WeekDayId == createOrUpdateSchedule.WeekDayId && s.StylistId == stylistId);
            if (schedule == null)
            {
                var createSchedule = Mapper.Map<CreateOrUpdateScheduleDTO, Schedule>(createOrUpdateSchedule);
                createSchedule.StylistId = stylistId;
                await UnitOfWork.Repository<Schedule>().AddAsync(createSchedule);
                return await Save();
            }


            schedule.StartTime = createOrUpdateSchedule.StartTime;
            schedule.EndTime = createOrUpdateSchedule.EndTime;

            UnitOfWork.Repository<Schedule>().Update(schedule);
            return await Save();

        }    
       
    }
}
