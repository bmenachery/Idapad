using Infrastructure.Identity.Models;

namespace Infrastructure.Interfaces
{
    public interface ITokenService
    {
         string CreateToken(AppUser user);
    }
}