using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Account
{
  [Table("UserRole", Schema = "account")]
  public class UserRole
  {
    [Key, ForeignKey("User"), Column(Order = 1)]
    public Guid UserId { get; set; }

    [Key, ForeignKey("Role"), Column(Order = 2)]
    public int RoleId { get; set; }
  }
}