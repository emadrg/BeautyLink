using API.Attributes;
using BL.Services;
using Common.Enums;
using DTOs.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [APIEndpoint]
    [Route("services")]
    public class ServiceEntityController : ControllerBase
    {
        private readonly ServiceEntityService ServiceEntityService;

        public ServiceEntityController(ServiceEntityService serviceEntityService)
        {
            ServiceEntityService = serviceEntityService;
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]

        public async Task<List<ServiceDTO>> GetServices()
        {
            return await ServiceEntityService.GetServices();
        }

        [HttpGet]
        [Route("{id}")]
        [APIEndpoint(HttpMethodTypes.Get)]

        public async Task<ServiceDTO> GetServiceById([FromQuery] int id)
        {
            return await ServiceEntityService.GetServiceById(id);
        }
    }
}
