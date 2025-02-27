using API.Attributes;
using BL.Services;
using Common.Enums;
using DTOs.ServiceStylist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [APIEndpoint]
    [Route("serviceStylists")]
    public class ServiceStylistController
    {
        private readonly ServiceStylistService ServiceStylistService;

        public ServiceStylistController(ServiceStylistService serviceStylistService)
        {
            ServiceStylistService = serviceStylistService;
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Authorize]
        public async Task <List<ServiceStylistDTO>> GetStylistServices()
        {
            return await ServiceStylistService.GetStylistServices();
        }

        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("update")]
        [Authorize]

        public async Task<int> EditStylistService(EditOrAddServiceStylistDTO serviceStylistDTO)
        {
            return await ServiceStylistService.EditStylistService(serviceStylistDTO);
        }

        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("create")]
        [Authorize]

        public async Task<int> AddStylistService (EditOrAddServiceStylistDTO serviceStylistDTO)
        {
            return await ServiceStylistService.AddStylistService(serviceStylistDTO);
        }

        [HttpDelete]
        [APIEndpoint(HttpMethodTypes.Delete)]
        [Route("delete/{id}")]

        public async Task<int?> DeteleServiceStylist (int id)
        {
            return await ServiceStylistService.DeteleServiceStylist(id);
        }

    }
}
