

using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Security.Claims;

namespace NewsApiServies.Auth.ClassStatic
{

    public class CurrentUser
    {
    
        public static string Id(HttpContext context)
        {
            string userId = context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            return userId;
        }
        public static string Role(HttpContext context)
        {
            string userId = context.User.FindFirst(ClaimTypes.Role)!.Value;

            return userId;
        }
    }
}
