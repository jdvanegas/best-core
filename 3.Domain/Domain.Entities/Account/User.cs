using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Account
{
  [Table("User", Schema = "account")]
  public class User
  {
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("Role")]
    public int RoleId { get; set; }

    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    //public ICollection<UserRole> Roles { get; set; }
  }
}