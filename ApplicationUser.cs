using Microsoft.AspNetCore.Identity;

namespace MyWebApp
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}