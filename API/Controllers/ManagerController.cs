using API.Attributes;
using BL.Services;
using Common.Enums;
using DTOs.Manager;
using DTOs.Reviews;
using DTOs.Stylist;
using DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [APIEndpoint]
    [Route("managers")]
    public class ManagerController : ControllerBase
    {
        private readonly ManagerService ManagerService;

        public ManagerController(ManagerService managerService)
        {
            ManagerService = managerService;
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]

        public async Task<ManagerDTO> GetManagerById(Guid id)
        {
            return await ManagerService.GetManagerById(id);
        }

        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("register")]
        [AllowAnonymous]
        [Validate<CreateManagerWithSalonDTO>]

        public async Task<int> RegisterManager([FromForm]CreateManagerWithSalonDTO newManager)
        {
            return await ManagerService.RegisterManager(newManager);
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("getAllStylistsFromSalon/{managerId}")]

        public async Task<List<StylistDTO>> GetStylistsFromManagersSalon(Guid managerId)
        {
            return await ManagerService.GetStylistsFromManagersSalon(managerId);
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("getManagerId/{userId}")]

        public async Task<Guid> GetManagerIdByUserId (Guid userId)
        {
            return await ManagerService.GetManagerIdByUserId(userId);
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("stylistsReviews/{managerId}")]

        public async Task<List<DisplayClientStylistReviewDTO>> GetStylistReviewsFromManagersSalon(Guid managerId)
        {
            return await ManagerService.GetStylistReviewsFromManagersSalon(managerId);
        }
    }
}
