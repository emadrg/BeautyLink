namespace DTOs.Users
{
    public class UserListItemDTO
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public int? ProfilePictureId { get; set; }

        public string? PhoneNumber {  get; set; }

    }
}
