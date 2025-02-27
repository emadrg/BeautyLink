using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
using DTOs.StylistUnavailableTime;
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
    public class UnavailableTimeService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public UnavailableTimeService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task <List<UnavailableTimeDTO>> GetUnavailableTimeByStylistId(Guid stylistId)
        {
            var unavailableTime = await UnitOfWork.Queryable<UnavailableTime>()
                .Where(s => s.StylistId == stylistId).ToListAsync();

            return Mapper.Map<UnavailableTime, UnavailableTimeDTO>(unavailableTime);
        }

        public async Task<int> CreateUnavailableTime (CreateUnavailableTimeDTO unavailableTimeDTO, Guid stylistId)
        {

            var unavailableTime = Mapper.Map<CreateUnavailableTimeDTO, UnavailableTime>(unavailableTimeDTO);

            unavailableTime.StylistId = stylistId;
            UnitOfWork.Repository<UnavailableTime>().Add(unavailableTime);
            return await Save();
        }
    }
}
