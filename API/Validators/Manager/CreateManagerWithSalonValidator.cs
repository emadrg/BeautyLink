using BL.Services;
using DTOs.Manager;
using DTOs.Stylist;
using Resources.ValidationMessages;
using System.Text.RegularExpressions;
using Utils;

namespace API.Validators.Manager
{
    public class CreateManagerWithSalonValidator : BaseValidator<CreateManagerWithSalonDTO>, IValidate<CreateManagerWithSalonDTO>
    {
        private readonly ManagerService ManagerService;

        private readonly UserService UserService;

        private readonly SalonService SalonService;

        public CreateManagerWithSalonValidator(ManagerService managerService, UserService userService, SalonService salonService)
        {

            ManagerService = managerService;
            SalonService = salonService;
            UserService = userService;

             ForProperty(p => p.Manager.SalonId)
                .Check(p => (p.Manager.SalonId != null && p.Salon == null) || (p.Salon != null && p.Manager.SalonId == null), ManagerValidationMessages.SalonisRequired);



            ForProperty(p =>"email")
               .Check(p => !string.IsNullOrEmpty(p.Manager.Email), UserValidationMessages.EmailRequired)
               .Check(p => p.Manager.Email.IsValidEmail(), UserValidationMessages.EmailNotValid)
               .Check(async p => !await UserService.EmailIsUsed(p.Manager.Email), UserValidationMessages.EmailAlreadyUsed);

            ForProperty(p => p.Manager.Password)
                .Check(p => !string.IsNullOrEmpty(p.Manager.Password), UserValidationMessages.PasswordRequired)
                .Check(p => Regex.IsMatch(p.Manager.Password, "^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$"), UserValidationMessages.PasswordNotValid);

            ForProperty(p => p.Manager.PhoneNumber)
                .Check(p => p.Manager.PhoneNumber?.IsValidPhoneNumber() ?? true, UserValidationMessages.PhoneNumberNotValid);

            ForProperty(p => p.Manager.SalonId)
                .Check(async p =>
                {
                    if (p.Manager.SalonId != null)
                    {
                        await SalonService.SalonExists((int)p.Manager.SalonId);
                    }
                    return true;
                }, StylistValidationMessages.SalonDoesntExist);


            ForProperty(p => p.Salon.Name)
               .Check(p => p.Salon != null ? !string.IsNullOrEmpty(p.Salon.Name) : true, SalonValidationMessages.NameRequired);
            ForProperty(p => p.Salon.Address)
                .Check(p => p.Salon != null ? !string.IsNullOrEmpty(p.Salon.Address) : true, SalonValidationMessages.AddressRequired);

        }

    }
}
