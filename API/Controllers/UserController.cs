using API.Attributes;
using BL.Services;
using Common.Enums;
using DTOs.Query;
using DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Controllers
{
    [ApiController]
    [Authorize]
    [APIEndpoint]
    public class UserController : ControllerBase
    {
        private readonly UserService UserService;

        public UserController(UserService userService)
        {
            UserService = userService;
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<UserDetailsDTO?> GetUserById(Guid id)
        {
            return await UserService.GetUserDetailsById(id);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("user/profilePicture/{id}")]
        public async Task<string?> GetUserProfilePicture(Guid id)
        {
            return await UserService.GetUserProfilePicture(id);
        }

        [HttpPost]
        [Route("users")]
        [APIEndpoint(HttpMethodTypes.Get)]
        public async Task<List<UserListItemDTO>> GetUsers(QueryDTO<LikeValueFilterDTO, InValueListFilterDTO, BetweenValuesFilterDTO> query)
        {
            return await UserService.GetUserList(query);
        }
        [HttpPost]
        [Route("user/register")]
        [AllowAnonymous]
        [Validate<RegisterUserDTO>]
        public async Task<int> RegisterUser([FromForm] RegisterUserDTO userDTO)
        {
            return await UserService.RegisterUser(userDTO);
        }
     
        [HttpPut]
        [Route("user/deactivate/{userId}")]
        [CheckRole(Roles.Admin)]
        public async Task<int?> DeactivateUser(Guid userId)
        {
            return await UserService.DeactivateUser(userId);
        }
        [HttpDelete]
        [Route("user/{userId}")]
        [CheckRole(Roles.Admin)]
        public async Task<int?> DeleteUser(Guid userId)
        {
            return await UserService.DeleteUser(userId);
        }

        [HttpGet]
        [Route("user/resetPassword/{userId}")]
        [APIEndpoint(HttpMethodTypes.Get)]
        public async Task<int> ResetPassword(Guid userId)
        {
            return await UserService.ResetPassword(userId);
        }

        [HttpGet]
        [Route("user/getAll/{skip}/{take}")]
        [APIEndpoint(HttpMethodTypes.Get)]

        public async Task<List<DispalyUserDTO>> GetAllUsers([FromQuery] int? roleId, int skip, int take)
        {
            return await UserService.GetAllUsers(roleId, skip, take);
        }

        [HttpPost]
        [Route("user/update")]
        [APIEndpoint(HttpMethodTypes.Post)]

        public async Task<int?> UpdateUserDetails (DispalyUserDTO userDTO)
        {
            return await UserService.UpdateUserDetails(userDTO);
        }

        
    }
}
