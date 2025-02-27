using API.Attributes;
using BL.Services;
using Common.Enums;
using DA.Entities;
using DTOs.Salon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [APIEndpoint]
    [Route("salons")]
    
    public class SalonController : ControllerBase
    {
        private readonly SalonService SalonService;

        public SalonController(SalonService salonService)
        {
            SalonService = salonService;
        }

        
        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Validate<CreateSalonDTO>]
        [Route("create")]

        public async Task<int> CreateSalon(CreateSalonDTO salonDTO)
        {
            return await SalonService.CreateSalon(salonDTO);
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        public async Task<List<SalonItemDTO>> GetSalons([FromQuery] int? serviceId, [FromQuery] int? skip, [FromQuery] int? take)
        {
            return await SalonService.GetSalons(serviceId, skip, take);
        }
        
        [HttpGet]
        [Route("getByCity")]
        [APIEndpoint(HttpMethodTypes.Get)]
        public async Task<List<SalonItemDTO>> GetSalonsByLocation([FromQuery] int cityId, [FromQuery] int? serviceId)
        {
            return await SalonService.GetSalonsByLocationAndServices(cityId, serviceId);
        }



        [HttpPut]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Validate<UpdatedSalonDTO>]

        public async Task<int?> UpdateSalon(UpdatedSalonDTO updatedSalonDTO)
        {
            return await SalonService.UpdateSalon(updatedSalonDTO);
        }

        [HttpDelete]
        [Route("{id}")]
        [APIEndpoint(HttpMethodTypes.Delete)]

        public async Task<int?> DeleteSalon(int id)
        {
            return await SalonService.DeleteSalon(id);
        }

        [HttpGet]
        [Route("{id}")]
        [APIEndpoint(HttpMethodTypes.Get)]

        public async Task<SalonDetailsDTO> GetSalonDetailsById(int id)
        {
            return await SalonService.GetSalonDetailsById(id);
        }
        
        [HttpGet]
        [Route("pictures/{id}")]
        [APIEndpoint(HttpMethodTypes.Get)]

        public async Task<List<string>> GetSalonPictures(int id, [FromQuery] int? skip, [FromQuery] int? take)
        {
            return await SalonService.GetSalonPictures(id, skip, take) ?? new List<string>();
        }

        [HttpGet]
        [Route("suggestions/{cityId}")]
        [APIEndpoint(HttpMethodTypes.Get)]

        public async Task<List<SalonSuggestionDTO>> GetSalonSuggestionsInUserArea(int cityId)
        {
            return await SalonService.GetSalonSuggestionsInUserArea(cityId);
        }
        
        [HttpGet]
        [Route("lastVisited/{id}")]
        [APIEndpoint(HttpMethodTypes.Get)]
        public async Task<SalonItemDTO> GetLastVisitedSalonByUserId(Guid id)
        {
            return await SalonService.GetLastVisitedSalonByUserId(id);
        }

        [HttpGet]
        [Route("hasClientVisited/{salonId}")]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Authorize]

        public async Task<bool> HasClientVisitedThisSalon(int salonId)
        {
            return await SalonService.HasClientVisitedThisSalon(salonId);
        }

        [HttpGet]
        [Route("getIdByManagerId/{managerId}")]
        [APIEndpoint(HttpMethodTypes.Get)]

        public async Task<int> GetSalonIdByManagerId(Guid managerId)
        {
            return await SalonService.GetSalonIdByManagerId(managerId);
        }

    }
}
