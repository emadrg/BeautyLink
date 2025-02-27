using API.Attributes;
using BL.Services;
using Common.Enums;
using DTOs.City;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [APIEndpoint]
    [Route("cities")]
    public class CityController : ControllerBase
    {
        private readonly CityService CityService;

        public CityController(CityService cityService)
        {
            CityService = cityService;
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]

        public async Task<List<CityDTO>> GetCities()
        {
            return await CityService.GetCities();
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("{countyId}")]
        public async Task<List<CityDTO>> GetCitiesByCountyId(int countyId)
        {
            return await CityService.GetCitiesByCountyId(countyId);
        }
    }
}
