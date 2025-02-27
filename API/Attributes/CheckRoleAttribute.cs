using Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Attributes
{
    public class CheckRoleAttribute : TypeFilterAttribute
    {
        public CheckRoleAttribute(Roles roleToCheck) : base(typeof(CheckRoleFilter))
        {
            Arguments = new object[] { roleToCheck };
        }
    }
    public class CheckRoleFilter: IAuthorizationFilter
    {
        private readonly Roles RoleToCheck;
        private readonly string RoleClaimName = "RoleId";
        public CheckRoleFilter(Roles roleToCheck) 
        {
            RoleToCheck = roleToCheck;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == RoleClaimName);
            if (claim == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            try
            {
                if((Roles)int.Parse(claim.Value) != RoleToCheck)
                {
                    context.Result = new ForbidResult();
                }
            }
            catch (Exception e)
            {
                context.Result = new ForbidResult();
                return;
            }
        } 
    }
}
