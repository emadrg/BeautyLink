using BL.Services;
using DTOs.Salon;
using DTOs.Users;
using Resources.ValidationMessages;
using Utils;

namespace API.Validators.Salon
{
    public class CreateSalonValidator : BaseValidator<SalonItemDTO>, IValidate<SalonItemDTO>
    {
        private readonly SalonService SalonService; 

        public CreateSalonValidator(SalonService salonService)
        {
            SalonService = salonService;

            ForProperty(p => p.Name)
                .Check(p => !string.IsNullOrEmpty(p.Name), SalonValidationMessages.NameRequired);
            ForProperty(p => p.Address)
                .Check(p => !string.IsNullOrEmpty(p.Address), SalonValidationMessages.AddressRequired);
        }
    }
}
