using AutoMapper;
using BL.UnitOfWork;
using Common.AppSettings;
using DA.Entities;
using DTOs.City;
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
    public class CityService : BaseService
    {
        private readonly ClaimsPrincipal CurrentUser;
        private readonly MapperService Mapper;
        public CityService(MapperService mapper, AppUnitOfWork unitOfWork, ILogger logger, IAppSettings appSettings, ClaimsPrincipal currentUser) : base(unitOfWork, logger, appSettings)
        {
            CurrentUser = currentUser;
            Mapper = mapper;
        }

        public async Task<List<CityDTO>> GetCities()
        {
            var cities = await UnitOfWork.Queryable<City>().ToListAsync();
            return Mapper.Map<City, CityDTO>(cities);
        }

        public async Task<List<CityDTO>> GetCitiesByCountyId(int countyId)
        {
            var cities = await UnitOfWork.Queryable<City>().Where(s => s.CountyId == countyId).ToListAsync();
            return Mapper.Map<City, CityDTO>(cities);
        }
    }
}
