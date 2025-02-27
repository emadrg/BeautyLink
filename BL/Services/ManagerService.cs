using BL.UnitOfWork;
using Common.AppSettings;
using DTOs.Manager;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utils;
using DTOs.Manager;


using DA.Entities;
using Microsoft.EntityFrameworkCore;
using DTOs.Users;
using DTOs.Salon;
using DTOs.Stylist;
using DTOs.Reviews;
using Common.Constants;

namespace BL.Services
{
    public class ManagerService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;
        private readonly UserService UserService;

        public ManagerService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser, UserService userService) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
            UserService = userService;
        }
        public async Task<ManagerDTO?> GetManagerById (Guid id)
        {
            var manager = await UnitOfWork.Queryable<Manager>().FirstOrDefaultAsync(m => m.Id == id);
            if (manager == null)
            {
                return null;
            }
            return Mapper.Map<Manager, ManagerDTO?>(manager);
        }

        public async Task<int> RegisterManager (CreateManagerWithSalonDTO newManagerWithSalon)
        {
            var salon = Mapper.Map<CreateSalonDTO, Salon>(newManagerWithSalon.Salon);

            var newDbManager = Mapper.Map<RegisterManagerDTO, Manager>(newManagerWithSalon.Manager);
            var newDbUser = Mapper.Map<RegisterManagerDTO, User>(newManagerWithSalon.Manager);

            newDbUser.CreatedBy = newDbUser.Id;
            newDbUser.LastModifiedBy = newDbUser.Id;
            newDbUser.Password = await SecurityExtensions.GetPasswordHashString(newDbUser.Email, newDbUser.Password, newDbUser.Id.ToString());
            newDbUser.StatusId = (int)Common.Enums.UserStatus.Active;

            if (newManagerWithSalon.Manager.ProfilePicture != null)
            {
                var picture = newManagerWithSalon.Manager.ProfilePicture;
                var pictureObject = new AppFile
                {
                    Extesion = picture.FileName.Split(".")[1],
                    Path = $"{FileUtils.BasePath}/{picture.FileName}",
                    Name = picture.FileName.Split(".")[0]

                };
                newDbUser.ProfilePicture = pictureObject;
                FileUtils.CreateFile(newManagerWithSalon.Manager.ProfilePicture);
            }

            newDbManager.User = newDbUser;
            newDbManager.Salon = salon; 

            UnitOfWork.Repository<Manager>().Add(newDbManager);

            return await Save();



        }
        public async Task<List<StylistDTO>> GetStylistsFromManagersSalon (Guid managerId)
        {
            var stylists = await UnitOfWork.Queryable<Stylist>()
                .Include(s => s.User)
                    .ThenInclude( u => u.ProfilePicture)
                .Include(s => s.Salon)
                    .ThenInclude(sal => sal.Managers)
                .Where(s => s.Salon.Managers.Any(m => m.Id == managerId))
                .ToListAsync();

            var mappedStylists = Mapper.Map<Stylist, StylistDTO>(stylists);

            foreach(var stylist in mappedStylists)
            {
                var profilePicture = await UserService.GetUserProfilePicture(stylist.UserId);
                stylist.ProfilePicture = $"{FileUtils.BasePath}\\{profilePicture}";
            }

           
            return mappedStylists;
        }

        public async Task<Guid> GetManagerIdByUserId (Guid userId)
        {
            var manager = await UnitOfWork.Queryable<Manager>()
                .Where(m => m.UserId == userId)
                .FirstOrDefaultAsync();
            return manager.Id;
        }

        public async Task<List<DisplayClientStylistReviewDTO>> GetStylistReviewsFromManagersSalon (Guid managerId)
        {

            var reviews = await UnitOfWork.Queryable<ReviewClientStylist>()
                .Include (r => r.Stylist)
                    .ThenInclude(s => s.Salon)
                        .ThenInclude(sal => sal.Managers.Where(m => m.Id == managerId))
                .ToListAsync();

            return Mapper.Map<ReviewClientStylist, DisplayClientStylistReviewDTO>(reviews);

        }
    }
}

