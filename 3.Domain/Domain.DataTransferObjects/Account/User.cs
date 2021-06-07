using System;

namespace Domain.DataTransferObjects.Account
{
  [Serializable]
  public class User
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string RoleName { get; set; }
    public string Token { get; set; }
  }
}