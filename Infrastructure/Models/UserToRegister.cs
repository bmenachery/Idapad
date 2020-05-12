namespace Infrastructure.Models
{
    public class UserToRegister
    {
        public int Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string FirmName { get; set; }

        public int FirmId { get; set; }

        public string EmailAddress { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}