using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
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
    public class ServiceStylistService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public ServiceStylistService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<List<ServiceStylistDTO>> GetStylistServices()
        {
            var stylistUserId = CurrentUser.Id();

            var services = await UnitOfWork.Queryable<ServiceStylist>()
                .Include(ss => ss.Stylist)
                    .ThenInclude(s => s.User)
                 .Where(ss => ss.Stylist.UserId == stylistUserId)
                 .Include(ss => ss.Service)
                .ToListAsync();

            return Mapper.Map<ServiceStylist, ServiceStylistDTO>(services);
        }

        public async Task<int> EditStylistService (EditOrAddServiceStylistDTO serviceStylistDTO)
        {
            var stylistUserId = CurrentUser.Id();
            var stylist = await UnitOfWork.Queryable<Stylist>()
                .Where(s => s.UserId == stylistUserId)
                .FirstOrDefaultAsync();

            var stylistId = stylist.Id;

            var serviceStylist = await UnitOfWork.Queryable<ServiceStylist>()
                .Where(ss => (ss.StylistId == stylistId) && (ss.ServiceId == serviceStylistDTO.ServiceId))
                .FirstOrDefaultAsync();

            serviceStylist.DurationMinutes = serviceStylistDTO.DurationMinutes;
            serviceStylist.Price = serviceStylistDTO.Price;

            UnitOfWork.Repository<ServiceStylist>().Update(serviceStylist);
            return await Save();

        }

        public async Task<int> AddStylistService (EditOrAddServiceStylistDTO serviceStylistDTO)
        {
            var stylistUserId = CurrentUser.Id();
            var stylist = await UnitOfWork.Queryable<Stylist>()
                .Where(s => s.UserId == stylistUserId)
                .FirstOrDefaultAsync();

            var stylistId = stylist.Id;

            var serviceStylist = Mapper.Map<EditOrAddServiceStylistDTO, ServiceStylist>(serviceStylistDTO);

            serviceStylist.StylistId = stylistId;
            UnitOfWork.Repository<ServiceStylist>().Add(serviceStylist);
            return await Save();
        }


        public async Task<int?> DeteleServiceStylist (int id)
        {
            var stylistUserId = CurrentUser.Id();
            var stylist = await UnitOfWork.Queryable<Stylist>()
                .Where(s => s.UserId == stylistUserId)
                .FirstOrDefaultAsync();

            var stylistId = stylist.Id;

            var dbServiceStylist = await UnitOfWork.Queryable<ServiceStylist>()
                .Include(ss => ss.Stylist)
                .Include(ss => ss.Service)
                .Where(ss => ss.ServiceId == id && ss.StylistId == stylistId)
                .FirstOrDefaultAsync();

            if (dbServiceStylist == null) 
            {
                return null;
            }

            UnitOfWork.Repository<ServiceStylist>().Remove(dbServiceStylist);
            return await Save();
        }
    }
}
