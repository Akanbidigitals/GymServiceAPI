using GymMembershipAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymMembershipAPI.DataAccess.DataContext
{
    public class ApplicationContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<GymSuperAdmin> GymSuperAdmins { get;set; }
        public DbSet<GymOwner> GymOwner { get; set; }
        public DbSet<GymMember> Members { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<HealthyTip> HealthyTip { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet <UserRole> UserRoles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Role>().HasData(new Role { Id = 1, Name = "SuperAdmin" }, new Role { Id = 2, Name = "GymOwner" }, new Role { Id = 3, Name = "GymMembers" });
            modelBuilder.Entity<UserRole>().HasKey(k => new { k.RoleId, k.UserId });
            modelBuilder.Entity<UserRole>().HasOne(k => k.User).WithMany(c => c.Roles).HasForeignKey(k => k.UserId);
            modelBuilder.Entity<UserRole>().HasOne(k=> k.Role).WithMany(c => c.Users).HasForeignKey(k=>k.RoleId);

        }

    }
}
