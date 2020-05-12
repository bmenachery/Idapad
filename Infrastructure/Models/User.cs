namespace Infrastructure.Models
{
    public class User: BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }

        public string FirmName { get; set; }

        public string FirmType { get; set; }

        public int FirmId { get; set; }


    }
}