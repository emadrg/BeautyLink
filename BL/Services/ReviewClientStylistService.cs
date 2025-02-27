using AutoMapper.Configuration.Annotations;
using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
using DTOs.Reviews;
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
    public class ReviewClientStylistService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public ReviewClientStylistService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<List<DisplayClientStylistReviewDTO>> GetStylistReviewsByStylistId (Guid stylistId)
        {
            var reviews = await UnitOfWork.Queryable<ReviewClientStylist>()
                .Include(s => s.Client)
                .Include(s => s.Stylist)
                .ThenInclude(s => s.User)
                .Where(s => s.StylistId == stylistId)
                .ToListAsync();
            return Mapper.Map<ReviewClientStylist, DisplayClientStylistReviewDTO>(reviews); 
        }

        public async Task<List<DisplayClientStylistReviewDTO>> GetStylistReviewsLeftByClient(Guid clientId)
        {

            var reviews = await UnitOfWork.Queryable<ReviewClientStylist>()
               
                .Include(s => s.Client)
                .Include(s => s.Stylist)
                .ThenInclude(s => s.User)
                .Where(s => s.ClientId == clientId)
                .ToListAsync();
            return Mapper.Map<ReviewClientStylist, DisplayClientStylistReviewDTO>(reviews);
        }

        public async Task<int> CreateClientStylistReview(CreateClientStylistReviewDTO reviewDTO)
        {
            var review = Mapper.Map<CreateClientStylistReviewDTO, ReviewClientStylist>(reviewDTO);
            var currentUserId = CurrentUser.Id();
            review.ClientId = currentUserId;
            UnitOfWork.Repository<ReviewClientStylist>().Add(review);
            return await Save();
        }

        public async Task<int> UpdateClientStylistReview(UpdateClientStylistReviewDTO reviewDTO)
        {
            var currentUserId = CurrentUser.Id();
            var review = await UnitOfWork.Queryable<ReviewClientStylist>()
                .FirstOrDefaultAsync(r => r.ClientId == currentUserId && r.StylistId == reviewDTO.StylistId);
            review.Text = reviewDTO.Text;
            review.Score = reviewDTO.Score;
            UnitOfWork.Repository<ReviewClientStylist>().Update(review);
            return await Save();
        }

        

    }
}
