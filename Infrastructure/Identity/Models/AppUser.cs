using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Models
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }

        public Address Address { get; set; }
    }
}