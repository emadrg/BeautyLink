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
    [Route("reviewsClientStylist")]
    public class ReviewClientStylistController : ControllerBase
    {
        private readonly ReviewClientStylistService ReviewClientStylistService;

        public ReviewClientStylistController(ReviewClientStylistService reviewClientStylistService)
        {
            ReviewClientStylistService = reviewClientStylistService;
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("{stylistId}")]

        public async Task<List<DisplayClientStylistReviewDTO>> GetClientStylistReviewByStylistId(Guid stylistId)
        {
            return await ReviewClientStylistService.GetStylistReviewsByStylistId(stylistId);
        } 
        
        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("leftByClient/{clientId}")]

        public async Task<List<DisplayClientStylistReviewDTO>> GetStylistReviewsLeftByClient(Guid clientId)
        {
            return await ReviewClientStylistService.GetStylistReviewsLeftByClient(clientId);
        } 
        
       
        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("create")]

        public async Task<int> CreateClientStylistReview(CreateClientStylistReviewDTO review)
        {
            return await ReviewClientStylistService.CreateClientStylistReview(review);
        }

        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("update")]

        public async Task<int> UpdateClientStylistReview(UpdateClientStylistReviewDTO review)
        {
            return await ReviewClientStylistService.UpdateClientStylistReview(review);
        }
    }
}
