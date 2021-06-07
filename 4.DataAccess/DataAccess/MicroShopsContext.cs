using Domain.Common;
using Domain.Entities.Account;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccess
{
  public class MicroShopsContext : DbContext
  {
    private readonly string _connectionString = "Server=.;Database=MicroShops;Trusted_Connection=True;";

    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      #region ModuleSeed

      modelBuilder.Entity<Role>().HasData(
        new Role
        {
          Id = 1,
          Name = "admin",
          Description = "Super Administrador"
        },
        new Role
        {
          Id = 2,
          Name = "store",
          Description = "Administrador de tienda y cliente"
        },
        new Role
        {
          Id = 3,
          Name = "customer",
          Description = "Cliente"
        }
      );

      modelBuilder.Entity<User>().HasData(
        new User
        {
          Id = Guid.NewGuid(),
          Name = "Juan David",
          LastName = "Vanegas Rodriguez",
          Email = "jdvanegas4@gmail.com",
          Phone = "3196780859",
          Password = "@Juan#7851202".HashPassword(),
          RoleId = 1
        }
      );

      #endregion ModuleSeed
    }
  }
}