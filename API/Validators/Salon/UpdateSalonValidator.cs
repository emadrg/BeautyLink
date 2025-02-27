using BL.Services;
using DTOs.Salon;
using Resources.ValidationMessages;
using Utils;

namespace API.Validators.Salon
{
    public class UpdatealonValidator : BaseValidator<UpdatedSalonDTO>, IValidate<UpdatedSalonDTO>
    {
        private readonly SalonService SalonService;

        public UpdatealonValidator(SalonService salonService)
        {
            SalonService = salonService;

            ForProperty(p => p.Id)
                .Check(p => p.Id != null, SalonValidationMessages.IdRequired);
            ForProperty(p => p.Name)
                .Check(p => !string.IsNullOrEmpty(p.Name), SalonValidationMessages.NameRequired);
            ForProperty(p => p.Address)
                .Check(p => !string.IsNullOrEmpty(p.Address), SalonValidationMessages.AddressRequired);
        }
    }
}
