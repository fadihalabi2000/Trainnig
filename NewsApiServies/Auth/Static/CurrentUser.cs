

using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Security.Claims;

namespace NewsApiServies.Auth.ClassStatic
{

    public class CurrentUser
    {
    
        public static int Id(HttpContext context)
        {
            int userId =int.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            return userId;
        }
        public static string Role(HttpContext context)
        {
            string role = context.User.FindFirst(ClaimTypes.Role)!.Value;

            return role;
        }
    }
}
