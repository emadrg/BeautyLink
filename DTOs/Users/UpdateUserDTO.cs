namespace DTOs.Users
{
    public class UpdateUserDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string Email { get; set; } = null!;

        public int? ProfilePictureId { get; set; }

        public string? PhoneNumber { get; set; }
    }
}

