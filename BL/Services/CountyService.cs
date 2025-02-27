using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
using DTOs.County;
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
    public class CountyService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public CountyService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<List<CountyDTO>> GetCounties()
        {
            var counties = await UnitOfWork.Queryable<County>().ToListAsync();
            return Mapper.Map<County, CountyDTO>(counties);
        }
    }
}
