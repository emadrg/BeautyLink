using API.Attributes;
using BL.Services;
using Common.Enums;
using DTOs.County;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [APIEndpoint]
    [Route("counties")]
    public class CountyController : ControllerBase
    {
        private readonly CountyService CountyService;
        public CountyController(CountyService countyService)
        {
            CountyService = countyService; 
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]

        public async Task<List<CountyDTO>> GetCounties()
        {
            return await CountyService.GetCounties();
        }
    }
}
