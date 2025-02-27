using API.Attributes;
using BL.Services;
using Common.Enums;
using DA.Entities;
using DTOs.Appointment;
using DTOs.Stylist;
using DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [APIEndpoint]
    [Route("stylists")]
    public class StylistController : ControllerBase
    {
        private readonly StylistService StylistService;

        private readonly AppointmentService AppointmentService;

        public StylistController(StylistService stylistService, AppointmentService appointmentService)
        {
            StylistService = stylistService;
            AppointmentService = appointmentService;
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("{id}")]

        public async Task<StylistDTO> GetStylistById (Guid id)
        {
            return await StylistService.GetStylistById(id);
        }

        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("register")]
        [AllowAnonymous]
        [Validate<RegisterStylistDTO>]

        public async Task<int> RegisterStylist([FromForm]RegisterStylistDTO newStylist)
        {
            return await StylistService.RegisterStylist(newStylist);
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("getStylistWithServices/{id}")]
        [AllowAnonymous]
        public async Task<StylistWithServicesDTO> GetStylistWithServices(Guid id)
        {
            return await StylistService.GetStylistWithServices(id);
        }


        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("getAppointments/{stylistId}")]

        public async Task<List<DisplayStylistAppointmentsDTO>> GetStylistAppointments(Guid stylistId)
        {
            return await AppointmentService.GetStylistAppointments(stylistId);
        }



        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("getStylistIdByUserId/{userId}")]

        public async Task<Guid> GetStylistIdByUserId (Guid userId)
        {
            return await StylistService.GetStylistIdByUserId(userId);
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("hasClientVisited/{stylistId}")]
        [Authorize]

        public async Task<bool> HasClientVisitedThisStylist(Guid stylistId)
        {
            return await StylistService.HasClientVisitedThisStylist(stylistId);
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("filerStylists/{startDate}/{endDate}/{serviceId}/{cityId}")]

        public async Task<List<StylistDTO>> GetStylistsByFiltering(DateTime startDate, DateTime endDate, int serviceId, int cityId)
        {
            return await StylistService.GetStylistsByFiltering(startDate, endDate, serviceId, cityId); 
        }
    }
}
