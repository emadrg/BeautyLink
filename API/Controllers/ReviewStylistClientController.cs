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
    [Route("reviewsStylistClient")]
    public class ReviewStylistClientController
    {
        private readonly ReviewStylistClientService ReviewStylistClientService;
        public ReviewStylistClientController(ReviewStylistClientService reviewStylistClientService)
        {
            ReviewStylistClientService = reviewStylistClientService;
        }

        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("create")]
        public async Task<int> CreateStylistClientReview(CreateStylistClientReviewDTO review)
        {
            return await ReviewStylistClientService.CreateStylistClientReview(review);
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("{clientId}")]
        public async Task<List<DisplayStylistClientReviewDTO>> GetStylistClientReviewsByClientId(Guid clientId)
        {
            return await ReviewStylistClientService.GetClientReviewsByClientId(clientId);
        }
        
        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]
        [Route("leftByStylist/{stylistId}")]
        public async Task<List<DisplayStylistClientReviewDTO>> GetClientReviewsLeftByStylist(Guid stylistId)
        {
            return await ReviewStylistClientService.GetClientReviewsLeftByStylist(stylistId);
        }

        [HttpPost]
        [APIEndpoint(HttpMethodTypes.Post)]
        [Route("update")]

        public async Task<int> UpdateStylistClientReview(UpdateStylistClientReviewDTO review)
        {
            return await ReviewStylistClientService.UpdateStylistClientReview(review);
        }
    }
}
