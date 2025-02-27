using API.Attributes;
using BL.Services;
using Common.Enums;
using DTOs.StylistUnavailableTime;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [APIEndpoint]
    [Route("unavailableTime")]
    public class UnavailableTimeController : ControllerBase
    {
        private readonly UnavailableTimeService UnavailableTimeService;
        public UnavailableTimeController(UnavailableTimeService unavailableTimeService)
        {
            UnavailableTimeService = unavailableTimeService;
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("{stylistId}")]

        public async Task<List<UnavailableTimeDTO>> GetUnavailableTimeByStylistId(Guid stylistId)
        {
            return await UnavailableTimeService.GetUnavailableTimeByStylistId(stylistId);
        }

        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("create/{stylistId}")]
        [Authorize]

        public async Task<int> CreateUnavailableTime(CreateUnavailableTimeDTO unavailableTimeDTO, Guid stylistId)
        {
            return await UnavailableTimeService.CreateUnavailableTime(unavailableTimeDTO, stylistId);
        }
    }
}
