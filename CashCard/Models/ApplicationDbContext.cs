using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CashCard.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<CashFlow>()
                .Map<CashIn>(m => m.Requires("CashType").HasValue("CashIn"))
                .Map<CashOutRegular>(m => m.Requires("CashType").HasValue("CastOutRegular"))
                .Map<CashOutIrregular>(m => m.Requires("CashType").HasValue("CastOutIregular"));
            
        }
        public DbSet<CashFlow> CashFlows { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<RegularQuiz> RegularQuizs { get; set; }
        //public DbSet<RegularDetail> RegularDetails { get; set; }
        public DbSet<CutOff> CutOffs { get; set; }
        public DbSet<RegularGroup> RegularGroup { get; set; }

      
    }
}