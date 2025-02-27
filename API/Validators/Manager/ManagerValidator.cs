using BL.Services;
using DTOs.Manager;
using Microsoft.OData.ModelBuilder;
using Resources.ValidationMessages;
using System.Text.RegularExpressions;
using Utils;

namespace API.Validators.Manager
{
    public class ManagerValidator : BaseValidator <RegisterManagerDTO>, IValidate<RegisterManagerDTO>
    {
        private readonly ManagerService ManagerService;

        private readonly UserService UserService;

        private readonly SalonService SalonService;

        public ManagerValidator(ManagerService managerService, UserService userService, SalonService salonService)
        {
            ManagerService = managerService;
            SalonService = salonService;
            UserService = userService;

            ForProperty(p => p.Email)
               .Check(p => !string.IsNullOrEmpty(p.Email), UserValidationMessages.EmailRequired)
               .Check(p => p.Email.IsValidEmail(), UserValidationMessages.EmailNotValid)
               .Check(async p => !await UserService.EmailIsUsed(p.Email), UserValidationMessages.EmailAlreadyUsed);

            ForProperty(p => p.Password)
                .Check(p => !string.IsNullOrEmpty(p.Password), UserValidationMessages.PasswordRequired)
                .Check(p => Regex.IsMatch(p.Password, "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$"), UserValidationMessages.PasswordNotValid);

            ForProperty(p => p.PhoneNumber)
                .Check(p => p.PhoneNumber?.IsValidPhoneNumber() ?? true, UserValidationMessages.PhoneNumberNotValid);

            ForProperty(p => p.SalonId)
                .Check(async p =>
                { 
                    if (p.SalonId != null)
                    { 
                        await SalonService.SalonExists((int)p.SalonId);
                    }
                    return true;
                }, StylistValidationMessages.SalonDoesntExist);
            
        }
    }
}
