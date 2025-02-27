using API.Attributes;
using BL.Services;
using Common.Enums;
using DA.Entities;
using DTOs.Appointment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [APIEndpoint]

    [Route("appointment")]
    public class AppointmentController : ControllerBase

    {
        private readonly AppointmentService AppointmentService;
        public AppointmentController(AppointmentService appointmentService)
        {
            AppointmentService = appointmentService;
        }

        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("create")]
        [Authorize]

        public async Task<int> CreateAppointment(CreateAppointmentDTO appointmentDTO)
        {
            return await AppointmentService.CreateAppointment(appointmentDTO);
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("{stylistId}")]
        [Authorize]
        [CheckRole(Roles.Client)]

        public async Task<List<AppointmentDTO>> GetAppointemntsByStylistId(Guid stylistId)
        {
            return await AppointmentService.GetStylistScheduleByStylistId(stylistId);
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("identify/{stylistId}/{startDate}")]

        public async Task<AppointmentDetailsDTO> GetAppointmentByStartDateAndStylistId(DateTime startDate, Guid stylistId)
        {
            return await AppointmentService.GetAppointmentByStartDateAndStylistId(startDate, stylistId);
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("clientAll")]
        [Authorize]

        public async Task<List<AppointmentDTO>> GetClientAppointments()
        {
            return await AppointmentService.GetClientAppointments();
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("stylistAll")]
        [Authorize]

        public async Task<List<AppointmentDTO>> GetStylistAppointments()
        {
            return await AppointmentService.GetStylistAppointments();
        }



        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("accept/{appointmentId}")]

        public async Task<int?> AcceptAppointment(int appointmentId)
        {
            return await AppointmentService.AcceptAppointment(appointmentId);
        }


        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("deny/{appointmentId}")]

        public async Task<int?> DenyAppointment(int appointmentId)
        {
            return await AppointmentService.DenyAppointment(appointmentId);
        }
    }
}
