using BL.Services;
using DTOs.Users;
using Resources.ValidationMessages;
using System.Text.RegularExpressions;
using Utils;

namespace API.Validators.User
{
    public class RegisterUserValidator : BaseValidator<RegisterUserDTO>, IValidate<RegisterUserDTO>
    {
        private readonly UserService UserService;

        public RegisterUserValidator(UserService userService)
        {
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
        }
    }
    public class UpdateUserValidator : BaseValidator<UpdateUserDTO>, IValidate<UpdateUserDTO>
    {
        private readonly UserService UserService;

        public UpdateUserValidator(UserService userService)
        {
            UserService = userService;

            ForProperty(p => p.Email)
                .Check(p => !string.IsNullOrEmpty(p.Email), UserValidationMessages.EmailRequired)
                .Check(p => p.Email.IsValidEmail(), UserValidationMessages.EmailNotValid)
                .Check(async p => !await UserService.EmailIsUsed(p.Email), UserValidationMessages.EmailAlreadyUsed);
        }
    }
}
