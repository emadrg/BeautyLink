using API.Attributes;
using BL.Services;
using Common.Enums;
using DA.Entities;
using DTOs.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [APIEndpoint]
    [Route("schedule")]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleService ScheduleService;
        public ScheduleController(ScheduleService scheduleService)
        {
            ScheduleService = scheduleService;
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("{stylistId}")]

        public async Task<List<ScheduleDTO>> GetScheduleByStylistId(Guid stylistId)
        {
            return await ScheduleService.GetScheduleByStylistId(stylistId);
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("weekdays")]
        [Authorize]
        [CheckRole(Roles.Stylist)]

        public async Task<List<WeekdayDTO>> GetWeekdays()
        {
            return await ScheduleService.GetWeekdays();
        }

        
        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("delete/{stylistId}/{weekDay}")]
        [Authorize]
        [CheckRole(Roles.Stylist)]

        public async Task<int> DeleteSchedule(int weekDay, Guid stylistId)
        {
            return await ScheduleService.DeleteSchedule(weekDay, stylistId);
        }

        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("createOrUpdate/{stylistId}")]
        [Authorize]
        [CheckRole(Roles.Stylist)]

        public async Task<int> CreateOrUpdate(CreateOrUpdateScheduleDTO createOrUpdateSchedule, Guid stylistId)
        {
            return await ScheduleService.CreateOrUpdate(createOrUpdateSchedule, stylistId);
        }

    }
}
