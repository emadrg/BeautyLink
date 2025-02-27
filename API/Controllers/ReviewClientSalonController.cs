using API.Attributes;
using BL.Services;
using Common.Enums;
using DTOs.Reviews;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [ApiController]
    [APIEndpoint]
    [Route("reviewsClientSalon")]
    public class ReviewClientSalonController
    {
        private readonly ReviewClientSalonService ReviewClientSalonService;

        public ReviewClientSalonController(ReviewClientSalonService reviewClientSalonService)
        {
            ReviewClientSalonService = reviewClientSalonService;
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("{salonId}")]

        public async Task<List<DisplayClientSalonReviewDTO>> GetClientSalonReviewBySalonId(int salonId, [FromQuery]int? skip, [FromQuery] int? take)
        {
            return await ReviewClientSalonService.GetSalonReviewsBySalonId(salonId, skip, take);
        }

        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("create")]
        [Authorize]

        public async Task<int> CreateClientSalonReview(CreateClientSalonReviewDTO review)
        {
            return await ReviewClientSalonService.CreateClientSalonReview(review);
        }

        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("update")]
        [Authorize]

        public async Task<int> UpdateClientSalonReview(UpdateClientSalonReviewDTO review)
        {
            return await ReviewClientSalonService.UpdateClientSalonReview(review);
        }
    }
}
