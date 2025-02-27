using API.Attributes;
using BL.Services;
using Common.AppSettings;
using Common.Enums;
using DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using Utils;

namespace API.Controllers
{
    [ApiController]
    [APIEndpoint]
    public class AuthenticationController: ControllerBase
    {
        private readonly IAppSettings AppSettings;
        private readonly UserService UserService;

        public AuthenticationController(IAppSettings appSettings, UserService userService) 
        {
            AppSettings = appSettings;
            UserService = userService;
        }

    
        [HttpPost]
        [Route("auth", Name = nameof(Authenticate))]
        [APIEndpoint(HttpMethodTypes.Get)]
        public async Task<AuthResponseDTO?> Authenticate(AuthCredentialsDTO authCredentials)
        {
            if (authCredentials.AppId == AppSettings.AuthSettings.UIClientId && authCredentials.AppKey == AppSettings.AuthSettings.UIClientSecret)
            {
                var user = await UserService.GetUserDetailsByCredentials(authCredentials.Id, authCredentials.Key);
                if(user == null)
                {
                    return null;
                }

                var jwt_token = AuthExtensions.BuildJWT(AppSettings.AuthSettings.Issuer, AppSettings.AuthSettings.SymmetricSecurityKey, user);
                return new AuthResponseDTO { jwt = jwt_token, userDetails = user };
            }
            else
            {
                return null;
            }
        }
    }
}
