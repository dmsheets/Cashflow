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
            modelBuilder.Entity<CashCard>()
                .Map<CashIn>(m => m.Requires("CashType").HasValue("CashIn"))
                .Map<CashOut>(m => m.Requires("CashType").HasValue("CastOut"));

        }
        public DbSet<CashCard> CashCards { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Quiz> Quizs { get; set; }
        //public DbSet<CashOutDetail> RegularDetails { get; set; }
        public DbSet<CutOff> CutOffs { get; set; }
        public DbSet<QuizGroup> RegularGroup { get; set; }
        public DbSet<Kendaraan> Kendaraan { get; set; }

      
    }
}