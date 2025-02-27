using AutoMapper;
using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
using DTOs.Role;
using DTOs.Service;
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
    public class ServiceEntityService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;

        public ServiceEntityService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<List<ServiceDTO>> GetServices()
        {
            var service = await UnitOfWork.Queryable<Service>().ToListAsync();
            return Mapper.Map<Service, ServiceDTO>(service);
        }

        public async Task<ServiceDTO> GetServiceById(int id)
        {
            var service = await UnitOfWork.Queryable<Service>().FirstOrDefaultAsync(s => s.Id == id);
            if (service == null)
            {
                return null;
            }
            return Mapper.Map<Service, ServiceDTO>(service);

        }

    }
}
