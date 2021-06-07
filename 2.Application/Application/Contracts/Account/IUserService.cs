using Domain.Common;
using Domain.DataTransferObjects.Account;
using System;
using System.Threading.Tasks;

namespace Application.Contracts.Account
{
  public interface IUserService : IApplicationService<Domain.Entities.Account.User>
  {
    Task<Result<User>> Authenticate(string email, string password);

    Task<Result<User>> GetData(Guid guid);

    Task<Result<User>> Register(UserSignup user);
  }
}