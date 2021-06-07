using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataTransferObjects.Account
{
  public class UserSignin
  {
    public string Email { get; set; }
    public string Password { get; set; }
  }
}