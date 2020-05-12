namespace Infrastructure.Models
{
    public class FirmUser: BaseEntity
    {
        public int FirmId { get; set; }
        public int UserId { get; set; }

        public string UserName { get; set; }
    }
}
