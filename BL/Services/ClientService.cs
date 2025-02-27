using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
using DTOs.Appointment;
using DTOs.Client;
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

namespace BL.Services
{
    public class ClientService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public ClientService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<ViewClientDetailsDTO> ViewClientDetails(Guid clientId)
        {
            var currentUserId = CurrentUser.Id();
            var client = await UnitOfWork.Queryable<User>()
                .Include(u => u.ProfilePicture)
                .Include(c => c.Appointments)
                    .ThenInclude(a => a.ServiceStylists)
                        .ThenInclude(ss => ss.Stylist)
                            .ThenInclude(s => s.User)
                .Include(c => c.Appointments)
                    .ThenInclude(a => a.ServiceStylists)
                        .ThenInclude(ss => ss.Service)
                .FirstOrDefaultAsync(c => c.Id == clientId); 
            var returnedClient = Mapper.Map<User, ViewClientDetailsDTO>(client);
           
            returnedClient.Appointments = Mapper.Map<Appointment, AppointmentDTO>(client.Appointments).ToList();

            client.Appointments.ToList().ForEach(a => 
                returnedClient.Appointments.FirstOrDefault(rca => rca.Id == a.Id).Services = Mapper.Map<ServiceStylist, ServiceStylistDTO>(a.ServiceStylists).Where(s => s.StylistId == currentUserId).ToList()
            );

            returnedClient.Appointments = returnedClient.Appointments.Where(a => a.Services.Count > 0).ToList();

            return returnedClient;
        }
    }
}
