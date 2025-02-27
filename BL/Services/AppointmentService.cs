using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
using DTOs.Appointment;
using DTOs.ServiceStylist;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Common.Enums;

namespace BL.Services
{
    public class AppointmentService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public AppointmentService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<int> CreateAppointment(CreateAppointmentDTO appointment)
        {
            var dbAppointment = Mapper.Map<CreateAppointmentDTO, Appointment>(appointment);
            var serviceStylists = new List<ServiceStylist>();
            foreach (var serviceStylist in appointment.Services)
            {
                serviceStylists.AddRange(UnitOfWork.Queryable<ServiceStylist>()
                .Where(ss => ss.ServiceId == serviceStylist.ServiceId && ss.StylistId == serviceStylist.StylistId)
                .ToList());
            }
            dbAppointment.ServiceStylists = serviceStylists;
            dbAppointment.ClientId = CurrentUser.Id();

            UnitOfWork.Repository<ServiceStylist>().AttachRange(dbAppointment.ServiceStylists.ToList());

            UnitOfWork.Repository<Appointment>().Add(dbAppointment);
            return await Save();
        }

        public async Task<List<AppointmentDTO>> GetStylistScheduleByStylistId(Guid stylistId)
        {
           var appointments = await UnitOfWork.Queryable<Appointment>()
                .Include(a => a.ServiceStylists)
                .Where(a => a.ServiceStylists.Any(ss => ss.StylistId == stylistId) && a.StatusId != (int)Common.Enums.AppointmentStatus.Denied)
                .ToListAsync();

            var returnedApps = new List<AppointmentDTO>();
            foreach (var app in appointments)
            {
                var mappedApp = Mapper.Map<Appointment, AppointmentDTO>(app);
                mappedApp.Services = Mapper.Map<ServiceStylist, ServiceStylistDTO>(app.ServiceStylists).ToList();
                returnedApps.Add(mappedApp);
            }
            return returnedApps;
        }

        public async Task<List<AppointmentDTO>> GetClientAppointments()
        {
            var currentUserId = CurrentUser.Id();

            var appointments = await UnitOfWork.Queryable<Appointment>()
                 .Include(a => a.ServiceStylists)
                    .ThenInclude(ss => ss.Service)
                     .Include(a => a.ServiceStylists)
                   .ThenInclude (ss => ss.Stylist)
                    .ThenInclude(s => s.User)
                   .Include(a => a.Status)
                 .Where(a => a.ClientId == currentUserId)
                 .OrderByDescending(a => a.StartDate)
                 .ToListAsync();
            
            var returnedApps = new List<AppointmentDTO>();

            foreach (var app in appointments)
            {
                var mappedApp = Mapper.Map<Appointment, AppointmentDTO>(app);
                mappedApp.Services = Mapper.Map<ServiceStylist, ServiceStylistDTO>(app.ServiceStylists).ToList();
               
                returnedApps.Add(mappedApp);
            }
            return returnedApps;
        }

        public async Task<List<AppointmentDTO>> GetStylistAppointments()
        {
            var currentUserId = CurrentUser.Id();

            var appointments = await UnitOfWork.Queryable<Appointment>()
                 .Include(a => a.Client)
                 .Include(a => a.ServiceStylists)
                    .ThenInclude(ss => ss.Service)
                  .Include(a => a.ServiceStylists)
                   .ThenInclude(ss => ss.Stylist)
                    .ThenInclude(s => s.User)
                   .Include(a => a.Status)
                   .Where(a => a.ServiceStylists.All(ss => ss.Stylist.UserId == currentUserId))
                   .OrderByDescending( a => a.StartDate)
                      .ToListAsync();

            var returnedApps = new List<AppointmentDTO>();

            foreach (var app in appointments)
            {
                var mappedApp = Mapper.Map<Appointment, AppointmentDTO>(app);
                mappedApp.Services = Mapper.Map<ServiceStylist, ServiceStylistDTO>(app.ServiceStylists).ToList();

                returnedApps.Add(mappedApp);
            }
            return returnedApps;
        }

        public async Task<List<DisplayStylistAppointmentsDTO>> GetStylistAppointments (Guid stylistId)
        {
            var appointments = await UnitOfWork.Queryable<Appointment>()
                .Include(a => a.Client)
                .Include (a => a.ServiceStylists)
                    .ThenInclude(ss => ss.Service)
                .Include(a => a.ServiceStylists)
                    .ThenInclude(ss => ss.Stylist)
                        .ThenInclude(s => s.User)
                .Where(a => a.ServiceStylists.Any(ss => ss.StylistId == stylistId) && a.StatusId != (int)Common.Enums.AppointmentStatus.Denied)
                .ToListAsync();

            var returnedApps = new List<DisplayStylistAppointmentsDTO>();
            foreach (var app in appointments)
            {
                var mappedApp = Mapper.Map<Appointment, DisplayStylistAppointmentsDTO>(app);
                mappedApp.Services = Mapper.Map<ServiceStylist, ServiceStylistDTO>(app.ServiceStylists).ToList();
                returnedApps.Add(mappedApp);
            }

            return returnedApps;
        }

        public async Task<AppointmentDetailsDTO> GetAppointmentByStartDateAndStylistId(DateTime startDate, Guid stylistId)
        {
            var appointment = await UnitOfWork.Queryable<Appointment>()
                 .Include(a => a.Client)
                .Include(a => a.ServiceStylists)
                    .ThenInclude(ss => ss.Stylist)
                        .Where(a => a.ServiceStylists.Any(ss => ss.StylistId == stylistId) && a.StartDate == startDate)
                .Include(a => a.ServiceStylists)
                    .ThenInclude(ss => ss.Service)
                        .FirstOrDefaultAsync();

            var mappedApp = Mapper.Map<Appointment, AppointmentDetailsDTO>(appointment);

            mappedApp.Services = Mapper.Map<ServiceStylist, ServiceStylistDTO>(appointment.ServiceStylists).ToList();
            
            return mappedApp;
        }

        public async Task<int?> AcceptAppointment(int appointmentId)
        {
            var appointment = await UnitOfWork.Queryable<Appointment>().FirstOrDefaultAsync(a => a.Id == appointmentId);
            if (appointment == null)
            {
                return null;
            }
            appointment.StatusId = 2;
            UnitOfWork.Repository<Appointment>().Update(appointment);
            return await Save();
        }

        public async Task<int?> DenyAppointment(int appointmentId)
        {
            var appointment = await UnitOfWork.Queryable<Appointment>().FirstOrDefaultAsync(a => a.Id == appointmentId);
            if (appointment == null)
            {
                return null;
            }
            appointment.StatusId = 3;
            UnitOfWork.Repository<Appointment>().Update(appointment);
            return await Save();
        }

    }
}
