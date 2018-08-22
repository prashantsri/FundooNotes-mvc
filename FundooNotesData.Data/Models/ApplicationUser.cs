using FundooNotesData.Data.Infrastructure;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FundooNotesData.Data.Models
{
    public class ApplicationUser : IdentityUser
    {

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string FirstName { get; set; }        
        public string LastName { get; set; }        
        public string Gender { get; set; }        
        public string DateOfBirth { get; set; }        
        public string Adhar { get; set; }        
        public string Countrycode { get; set; }        
        //public string PhoneNumber { get; set; }        
        public string Address { get; set; }
        public string Line2 { get; set; }        
        public string State { get; set; }        
        public string City { get; set; }        
        public string Postalcode { get; set; }        
        //public string UserName { get; set; }        
        public string Password { get; set; }
        public string Profile { get; set; }

        //ADD birthdate for facebook 
        public System.DateTime? BirthDate { get; set; }

        internal async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager userManager, string authenticationType)
        {
            var userIdentity = await userManager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }


    }
}
