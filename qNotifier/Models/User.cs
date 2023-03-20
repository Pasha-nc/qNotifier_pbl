using Microsoft.AspNetCore.Identity;

namespace qNotifier.Models
{
    public class User : IdentityUser
    {
        public int ClientTimeZoneOffset { get; set; }
        public List<UserRecord>? Records { get; set; }
       
    }
}
