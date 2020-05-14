using System.Linq;
using System.Security.Claims;


namespace Api.Extensions
{
    public static class ClaimsPrincipleExtensions
    {
        public static string RetrieveEmailFromPrincipal(this ClaimsPrincipal user)
        {
            return user?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        }
    }
}