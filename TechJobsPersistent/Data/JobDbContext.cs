using TechJobsPersistent.Models;
using Microsoft.EntityFrameworkCore;

namespace TechJobsPersistent.Data
{
    public class JobDbContext : DbContext
    {
        public DbSet<Job> Jobs { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }//db set that stores job skills for querying

        public JobDbContext(DbContextOptions<JobDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {//custom configures models 
            modelBuilder.Entity<JobSkill>()
                .HasKey(j => new { j.JobId, j.SkillId });
            //specifies JobSkills has a compound key of the pair of JobId and Skill Id 
        }
    }
}
