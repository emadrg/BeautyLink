using BL.UnitOfWork;
using Common.AppSettings;
using Common.Interfaces;
using DA.Entities;
using DTOs.Salon;
using DTOs.ServiceStylist;
using DTOs.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data.Common;
using System.Linq.Expressions;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using Utils;

namespace BL.Services
{
    public class SalonService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public SalonService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<SalonItemDTO?> GetSalonById(int id)
        {
            var salon = await UnitOfWork.Queryable<Salon>().FirstOrDefaultAsync(s => s.Id == id);
            if (salon == null)
            {
                return null;
            }
            return Mapper.Map<Salon, SalonItemDTO>(salon);
        }

        public async Task<List<SalonItemDTO>> GetSalons(int? serviceId, int? skip, int? take)
        {
            var salon = await UnitOfWork.Queryable<Salon>()
                .Include(e => e.City)
                .Include(e => e.County)
                .Where(e => serviceId == null || e.Stylists.Any(s => s.ServiceStylists.Any(ss => ss.ServiceId == serviceId)))
                .Skip(skip ?? 0)
                .Take(take ?? 1000)
                .ToListAsync();
            return Mapper.Map<Salon, SalonItemDTO>(salon);
        }

        public async Task<List<SalonItemDTO>> GetSalonsByLocationAndServices(int cityId, int? serviceId)
        {
            var salons = await UnitOfWork.Queryable<Salon>()
                .Where(e => e.CityId == cityId)
                .Where(e => serviceId == null || e.Stylists.Any(s => s.ServiceStylists.Any(ss => ss.ServiceId == serviceId)))
                .ToListAsync();
            return Mapper.Map<Salon, SalonItemDTO>(salons);
        }


        public async Task<List<SalonSuggestionDTO>> GetSalonSuggestionsInUserArea(int cityId)
        {
            var salonSuggestions = await UnitOfWork.Queryable<Salon>()
               .Where(s => s.CityId == cityId)
               .Include(s => s.ReviewClientSalons)
               .Include(s => s.City)
               .Include(s => s.County)
               .Select(s => new SalonSuggestionDTO
               {
                   Id = s.Id,
                   Name = s.Name,
                   City = s.City.Name,
                   County = s.County.Name,
                   Address = s.Address ?? string.Empty,
                   ReviewsNumber = s.ReviewClientSalons.Count(),
                   AvgScore = s.ReviewClientSalons.Average(rcs => rcs.Score)
               })
               .Where(s => s.ReviewsNumber > Common.Constants.SalonConstants.MinNumberOfReviews)
               .OrderByDescending(s => s.AvgScore)
               .Take(Common.Constants.SalonConstants.NumberOfDisplayedSuggestions)
               .ToListAsync();
            
            return salonSuggestions;
        }


        public async Task<int> CreateSalon(CreateSalonDTO salonDTO)
        {    
            var salon = Mapper.Map<CreateSalonDTO, Salon>(salonDTO);
            UnitOfWork.Repository<Salon>().Add(salon);
            return await Save();
        }

        public async Task<int?> UpdateSalon(UpdatedSalonDTO updatedSalonDTO)
        {
            var dbSalon = await UnitOfWork.Queryable<Salon>().FirstOrDefaultAsync(s => s.Id == updatedSalonDTO.Id);
            if (dbSalon == null)
            {
                return null;
            }
            dbSalon = Mapper.Map(updatedSalonDTO, dbSalon);
            
            UnitOfWork.Repository<Salon>().Update(dbSalon);
            return await Save();
        }

        public async Task<int?> DeleteSalon (int id)
        {
            var dbSalon = await UnitOfWork.Queryable<Salon>().FirstOrDefaultAsync(s => s.Id == id);
            if (dbSalon == null)
            {
                return null;
            }
           
            UnitOfWork.Repository<Salon>().Remove(dbSalon);
            return await Save();
        }

        public async Task<SalonDetailsDTO> GetSalonDetailsById(int id)
        {
            var salon = await UnitOfWork.Queryable<Salon>()
             .Include(e => e.City)
             .Include(e => e.County)
             .FirstOrDefaultAsync(e => e.Id == id);

            var serviceStylists = await UnitOfWork.Queryable<Salon>()
                .Where(e => e.Id == id)
                .SelectMany(e => e.Stylists)
                .SelectMany(e => e.ServiceStylists)
                .Include(e => e.Service)
                .Include( e => e.Stylist).ThenInclude( e => e.User)
                .ToListAsync();

            var salonDetails = Mapper.Map<Salon, SalonDetailsDTO>(salon);
            salonDetails.ServiceStylists = Mapper.Map<ServiceStylist, ServiceStylistDTO>(serviceStylists);

            return salonDetails;
        }

        public async Task<List<string>> GetSalonPictures (int salonId, int? skip, int? take)
        {
            var paths = await UnitOfWork.Queryable<Salon>()
                .Where(s => s.Id == salonId)
                .Include(s => s.Files)
                .Select(s => s.Files.Select(f => f.Path))
                .Skip(skip ?? 0)
                .Take(take ?? 1000)
                .FirstOrDefaultAsync() as List<string>;

            if (paths == null)
            {
                return null;
            }

            return paths;
        }

        public async Task<bool> SalonExists(int salonId)
        {
            return await UnitOfWork.Queryable<Salon>().AnyAsync(s => s.Id == salonId);
        }


        public async Task<SalonItemDTO> GetLastVisitedSalonByUserId (Guid userId)
        {
            var salonItem = await UnitOfWork.Queryable<Appointment>()
               .Include(a => a.Client)
               .Include(a => a.ServiceStylists).ThenInclude(ss => ss.Stylist).ThenInclude(s => s.Salon).ThenInclude(s => s.City)
                .Where(a => a.Client.Id == userId)
                .OrderByDescending(a => a.StartDate)
                .Select(a => a.ServiceStylists.Select(ss => ss.Stylist.Salon).FirstOrDefault())
                .Select(a => new SalonItemDTO
                {
                    Id = a.Id,
                    Address = a.Address,
                    Name = a.Name,
                    City = a.City.Name,
                    County = a.County.Name,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude
                })
               .FirstOrDefaultAsync();

            return salonItem ?? new SalonItemDTO();
        }

        public async Task<int> GetSalonIdByManagerId (Guid managerId)
        {
            var salon = await UnitOfWork.Queryable<Salon>()
                .Include(s => s.Managers)
                .Where(s => s.Managers.Any(m => m.Id == managerId))
                .FirstOrDefaultAsync();
            return salon.Id;
                
        }

        public async Task<bool> HasClientVisitedThisSalon (int salonId)
        {
            var currentUserId = CurrentUser.Id();

            var appointment = await UnitOfWork.Queryable<Appointment>()
                .Where(a => a.ClientId == currentUserId && a.EndDate < DateTime.Now && a.StatusId == 2)
                .Include(a => a.ServiceStylists)
                    .ThenInclude(ss => ss.Stylist)
                .Where(a => a.ServiceStylists.Any(ss => ss.Stylist.SalonId == salonId))
                .FirstOrDefaultAsync();

            return (appointment != null);
        }
    }
}