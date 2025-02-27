using Utils;

namespace DTOs.Users
{
    public class UserDetailsDTO: IAuthUser
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;

        public byte RoleId { get; set; }

        public string? ProfilePicture { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
