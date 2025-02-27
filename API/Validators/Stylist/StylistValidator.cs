using BL.Services;
using DTOs.Stylist;
using DTOs.Users;
using Resources.ValidationMessages;
using System.Text.RegularExpressions;
using Utils;

namespace API.Validators.Stylist
{
    public class StylistValidator : BaseValidator<RegisterStylistDTO>, IValidate<RegisterStylistDTO>
    {
        private readonly StylistService StylistService;

        private readonly UserService UserService;

        private readonly SalonService SalonService;

        private readonly ServiceEntityService ServiceEntityService;
        public StylistValidator(StylistService stylistService, UserService userService, SalonService salonService, ServiceEntityService serviceEntityService)
        {
            StylistService = stylistService;

            UserService = userService;

            SalonService = salonService;

            ServiceEntityService = serviceEntityService;

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
                .Check(async p => await SalonService.SalonExists(p.SalonId), StylistValidationMessages.SalonDoesntExist);
            SalonService = salonService;

            ForProperty(p => p.Services)
                .Check(async p => await StylistService.SelectedServicesExist(p), StylistValidationMessages.ServiceDoesntExist);
        }
    }
    }
