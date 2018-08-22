using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using FundooNotesData.Data.Models;

namespace FundooNotesData.Data.Infrastructure
{


    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<tblNotes> tblNotes { get; set; }
        public DbSet<tblLabel> tblLabels { get; set; }
        public DbSet<tblLabelNotes> tblLabelNotes { get; set; }
        public DbSet<tblCollaborator> tblCollaborators { get; set; }

    }
}