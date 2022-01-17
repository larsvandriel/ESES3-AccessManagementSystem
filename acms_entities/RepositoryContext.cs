using AccessManagementSystem.Entities.Configurations;
using AccessManagementSystem.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessManagementSystem.Entities
{
    public class RepositoryContext: DbContext
    {
        public RepositoryContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Accessor> Accessors { get; set; }
        public DbSet<AccessSetting> AccessSettings { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ElectronicLock> ElectronicLocks { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeFunction> EmployeeFunctions { get; set; }
        public DbSet<Entrance> Entrances { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accessor>().ToTable(nameof(Accessor));
            modelBuilder.ApplyConfiguration(new AccessSettingConfiguration());
            modelBuilder.Entity<Department>().ToTable(nameof(Department));
            modelBuilder.ApplyConfiguration(new ElectronicLockConfiguration());
            modelBuilder.Entity<Employee>().ToTable(nameof(Employee));
            modelBuilder.Entity<EmployeeFunction>().ToTable(nameof(EmployeeFunction));
            modelBuilder.ApplyConfiguration(new EntranceConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new RoomConfiguration());
        }
    }
}