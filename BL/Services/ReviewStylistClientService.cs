using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
using DTOs.Reviews;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace BL.Services
{
    public class ReviewStylistClientService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public ReviewStylistClientService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<int> CreateStylistClientReview(CreateStylistClientReviewDTO reviewDTO)
        {
            var review = Mapper.Map<CreateStylistClientReviewDTO, ReviewStylistClient>(reviewDTO);
            var currentUserId = CurrentUser.Id();
            var stylistId = await UnitOfWork.Queryable<Stylist>().Where(s => s.UserId == currentUserId).Select(s => s.Id).FirstOrDefaultAsync();
            review.StylistId = stylistId;
            UnitOfWork.Repository<ReviewStylistClient>().Add(review);
            return await Save();
        }
        public async Task<List<DisplayStylistClientReviewDTO>> GetClientReviewsByClientId(Guid clientId)
        {
            var reviews = await UnitOfWork.Queryable<ReviewStylistClient>()
                .Include(s => s.Client)
                .Include(s => s.Stylist)
                .ThenInclude(s => s.User)
                .Where(s => s.ClientId == clientId)
                .ToListAsync();
            return Mapper.Map<ReviewStylistClient, DisplayStylistClientReviewDTO>(reviews);
        }

        public async Task<List<DisplayStylistClientReviewDTO>> GetClientReviewsLeftByStylist(Guid stylistId)
        {
            var reviews = await UnitOfWork.Queryable<ReviewStylistClient>()
                .Include(s => s.Client)
                .Include(s => s.Stylist)
                .ThenInclude(s => s.User)
                .Where(s => s.StylistId == stylistId)
                .ToListAsync();
            return Mapper.Map<ReviewStylistClient, DisplayStylistClientReviewDTO>(reviews);
        }

        public async Task<int> UpdateStylistClientReview(UpdateStylistClientReviewDTO reviewDTO)
        {
            var currentUserId = CurrentUser.Id();
            
            var review = await UnitOfWork.Queryable<ReviewStylistClient>()
                .Include(r => r.Stylist)
                .FirstOrDefaultAsync(r => r.Stylist.UserId == currentUserId && r.ClientId == reviewDTO.ClientId);
            review.Text = reviewDTO.Text;
            review.Score = reviewDTO.Score;
            UnitOfWork.Repository<ReviewStylistClient>().Update(review);
            return await Save();
        }



    }
}
