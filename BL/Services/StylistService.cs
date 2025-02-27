using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
using DTOs.Appointment;
using DTOs.ServiceStylist;
using DTOs.Stylist;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace BL.Services
{
    public class StylistService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public StylistService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<StylistDTO> GetStylistById(Guid id)
        {
            var stylist = await UnitOfWork.Queryable<Stylist>()
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.Id == id);
            return Mapper.Map<Stylist, StylistDTO>(stylist);

        }

        public async Task<int> RegisterStylist (RegisterStylistDTO newStylist)
        {
            var newdbStylist = Mapper.Map<RegisterStylistDTO, Stylist>(newStylist);
            var newDbUser = Mapper.Map<RegisterStylistDTO, User>(newStylist);

            newDbUser.CreatedBy = newDbUser.Id;
            newDbUser.LastModifiedBy = newDbUser.Id;
            newDbUser.Password = await SecurityExtensions.GetPasswordHashString(newDbUser.Email, newDbUser.Password, newDbUser.Id.ToString());
            newDbUser.StatusId = (int)Common.Enums.UserStatus.Active;

            if (newStylist.ProfilePicture != null)
            {
                var picture = newStylist.ProfilePicture;
                var pictureObject = new AppFile
                {
                    Extesion = picture.FileName.Split(".")[1],
                    Path = $"{FileUtils.BasePath}{picture.FileName}",
                    Name = picture.FileName.Split(".")[0]

                };
                newDbUser.ProfilePicture = pictureObject;
                FileUtils.CreateFile(newStylist.ProfilePicture);
            }

            newdbStylist.User = newDbUser;

            UnitOfWork.Repository<Stylist>().Add(newdbStylist);

            return await Save();


        }

        public async Task<bool> ServiceExists(int serviceId)
        {
            return await UnitOfWork.Queryable<Service>().AnyAsync(s => s.Id == serviceId);
        }

        public async Task<bool> SelectedServicesExist(RegisterStylistDTO registerStylistDTO)
        {
            var services = registerStylistDTO.Services;
            foreach (var service in services)
            {
                if (await ServiceExists(service) == false) 
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<StylistWithServicesDTO> GetStylistWithServices(Guid stylistId)
        {
            var stylistWithSevices = await UnitOfWork.Queryable<Stylist>() 
                .Where(e => e.Id == stylistId)
                .Include(s => s.User)
                .Include(s => s.ServiceStylists)
                .ThenInclude(s => s.Service)
                .FirstOrDefaultAsync();

            var stylistWithServicesDTO = Mapper.Map<Stylist, StylistWithServicesDTO>(stylistWithSevices);
            stylistWithServicesDTO.Services = Mapper.Map<ServiceStylist, ServiceStylistDTO>(stylistWithSevices.ServiceStylists.ToList());

            return stylistWithServicesDTO;
        }

        public async Task<Guid> GetStylistIdByUserId (Guid userId)
        {
            var stylist = await UnitOfWork.Queryable<Stylist>()
                .Where(s => s.UserId == userId)
                .FirstOrDefaultAsync();

            return stylist.Id;
        }

        public async Task<bool> HasClientVisitedThisStylist (Guid stylistId)
        {
            var currentUserId = CurrentUser.Id();

            var appointments = await UnitOfWork.Queryable<Appointment>()
                .Where(a => a.ClientId == currentUserId && a.EndDate < DateTime.Now && a.StatusId == 2)
                .Include(a => a.ServiceStylists)
                    .ThenInclude(ss => ss.Stylist)
                .Where(a => a.ServiceStylists.All(ss => ss.Stylist.Id == stylistId))
                .ToListAsync();

            return appointments.Count != 0;
        }


        public async Task<bool> IsStylistAvailableSchedule(DateTime startDate, DateTime endDate, Guid stylistId, int serviceId)
        
        {
            var servicesByStylist = await UnitOfWork.Queryable<ServiceStylist>()
                .Where(s => s.StylistId == stylistId)
                .ToListAsync();
            
            var totalAvailableTime = new List<List<Schedule>>();

            foreach (var ss in servicesByStylist)
            {
                var availableTime = await UnitOfWork.Queryable<Schedule>()
                    .Where(s => s.StylistId == stylistId)
                    .ToListAsync();

                foreach(var at in availableTime)
                {
                    var availableTimeBySchedule = await UnitOfWork.Queryable<Schedule>()
                        .Where(s => s.StylistId == stylistId)
                        .Where(s => s.WeekDayId == (int)startDate.DayOfWeek + 1)
                        .FirstOrDefaultAsync();

                    if (availableTimeBySchedule != null
                        && availableTimeBySchedule.StartTime <= TimeOnly.FromTimeSpan(startDate.TimeOfDay)
                        && (availableTimeBySchedule.EndTime >= TimeOnly.FromTimeSpan(endDate.TimeOfDay)))
                    {
                        totalAvailableTime.Add(availableTime);
                    }
                }

              
            }

            return (totalAvailableTime.Count > 0);
        }

        public async Task<bool> IsStylistAvailableUnavailableTime(DateTime startDate, DateTime endDate, Guid stylistId, int serviceId)
        {
            var servicesByStylist = await UnitOfWork.Queryable<ServiceStylist>()
                .Where(s => s.StylistId == stylistId)
                .ToListAsync();

            var totalAvailableTime = new List<List<Stylist>>();

            foreach(var ss in servicesByStylist)
            {
                var availableTime = await UnitOfWork.Queryable<Stylist>()
                    .Include(s => s.UnavailableTimes)
                    .Where(s => s.UnavailableTimes.Any(ut => (ut.EndDate <= startDate) ||
                                                         (ut.StartDate >= startDate.AddMinutes(ss.DurationMinutes))))
                    .ToListAsync();
                if(availableTime.Count > 0)
                {
                    totalAvailableTime.Add(availableTime);
                }
                
            }
            return (totalAvailableTime.Count > 0);
        }

        public async Task<bool> IsStylistAvailableAppointments(DateTime startDate, DateTime endDate, Guid stylistId, int serviceId)
        {
            var servicesByStylist = await UnitOfWork.Queryable<ServiceStylist>()
                .Where(s => s.StylistId == stylistId)
                .ToListAsync();

            var totalAvailableTime = new List<List<Appointment>>();

            foreach(var ss in servicesByStylist)
            {
                var availableTime = await UnitOfWork.Queryable<Appointment>()
                    .Include(a => a.ServiceStylists)
                    .Where(a => (a.EndDate <= startDate) || (a.StartDate >= startDate.AddMinutes(ss.DurationMinutes)))
                    .ToListAsync();

                if (availableTime.Count > 0)
                {
                    totalAvailableTime.Add(availableTime);
                }
            }
            return (totalAvailableTime.Count > 0);
        }

        public async Task<List<StylistDTO>> GetStylistsByFiltering (DateTime startDate, DateTime endDate, int serviceId, int cityId)
        {
            var stylists = await UnitOfWork.Queryable<Stylist>()
                .Include(s => s.ReviewClientStylists)
                .Include(s => s.Salon)
                    .Where(s => s.Salon.CityId == cityId)
                .Include(s => s.User)
                    .ThenInclude(u => u.ProfilePicture)
                .Include(s => s.ServiceStylists)
                .Where(s => s.ServiceStylists.Any(ss => ss.ServiceId == serviceId))
                .ToListAsync();

            var availableStylists = new List<StylistDTO>();

            foreach(var stylist in stylists)
            {
                if((await IsStylistAvailableSchedule(startDate, endDate, stylist.Id, serviceId)) && 
                    (await IsStylistAvailableUnavailableTime(startDate, endDate, stylist.Id, serviceId)) &&
                    (await IsStylistAvailableAppointments(startDate, endDate, stylist.Id, serviceId))) 
                {
                    var mappedStylist = Mapper.Map<Stylist, StylistDTO>(stylist);
                    mappedStylist.AverageScore = stylist.ReviewClientStylists.Count > 0 ? stylist.ReviewClientStylists.Average(s => s.Score) : 0;
                    availableStylists.Add(mappedStylist); 
                }
                
            }

            return availableStylists;

        }

    }
}
