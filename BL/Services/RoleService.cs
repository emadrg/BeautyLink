using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
using DTOs.Role;
using DTOs.Salon;
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
    public class RoleService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public RoleService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<List<RoleDTO>> GetRoles() 
        {
            var role = await UnitOfWork.Queryable<Role>().ToListAsync();
            return Mapper.Map<Role, RoleDTO>(role);
        }
    }

}
