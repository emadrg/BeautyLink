using API.Attributes;
using BL.Services;
using Common.Enums;
using DTOs.Role;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [APIEndpoint]
    [Route("roles")]
    public class RoleController : ControllerBase
    {

        private readonly RoleService RoleService;
        public RoleController(RoleService roleService) 
        { 
            RoleService = roleService;
        }

        [HttpGet]
        [APIEndpoint(HttpMethodTypes.Get)]

        public async Task<List<RoleDTO>> GetRoles()
        {
            return await RoleService.GetRoles();
        }
    }
}
