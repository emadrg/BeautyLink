using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
using DTOs.Reviews;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace BL.Services
{
    public class ReviewClientSalonService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public ReviewClientSalonService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<List<DisplayClientSalonReviewDTO>> GetSalonReviewsBySalonId(int salonId, int? skip, int? take)
        {
            var review = await UnitOfWork.Queryable<ReviewClientSalon>()
                .Include(s => s.Client)
                .Where(s => s.SalonId == salonId)
                .Skip(skip ?? 0)
                .Take(take ?? 10)
                .ToListAsync();
            return Mapper.Map<ReviewClientSalon, DisplayClientSalonReviewDTO>(review);
        }

        public async Task<int> CreateClientSalonReview(CreateClientSalonReviewDTO reviewDTO)
        {
            var currentUserId = CurrentUser.Id();
            var review = Mapper.Map<CreateClientSalonReviewDTO, ReviewClientSalon>(reviewDTO);
            review.ClientId = currentUserId;
            UnitOfWork.Repository<ReviewClientSalon>().Add(review);
            return await Save();
        }

        public async Task<int> UpdateClientSalonReview(UpdateClientSalonReviewDTO reviewDTO)
        {
            var currentUserId = CurrentUser.Id();
            var review = await UnitOfWork.Queryable<ReviewClientSalon>()
                .FirstOrDefaultAsync(r => r.ClientId == currentUserId && r.SalonId == reviewDTO.SalonId);
            review.Text = reviewDTO.Text;
            review.Score = reviewDTO.Score;
            UnitOfWork.Repository<ReviewClientSalon>().Update(review);
            return await Save();
        }


    }
}
