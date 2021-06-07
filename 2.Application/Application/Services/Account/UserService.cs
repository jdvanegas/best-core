using Application.Contracts.Account;
using AutoMapper;
using Domain.Common;
using Domain.DataTransferObjects.Account;
using Domain.Entities.Account;
using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Account
{
  public class UserService : ApplicationService<Domain.Entities.Account.User>, IUserService
  {
    private readonly AppSettings _appSettings;

    public UserService(IRepository<Domain.Entities.Account.User> userRepository, IMapper mapper,
      IOptions<AppSettings> appSettings) : base(userRepository, mapper)
    {
      _appSettings = appSettings.Value;
    }

    public async Task<Result<Domain.DataTransferObjects.Account.User>> Authenticate(
      string email, string password)
    {
      var response = new Result<Domain.DataTransferObjects.Account.User>();
      password = password.HashPassword();
      var result = await Queryable().Include("Role")
        .FirstOrDefaultAsync(x => x.Email == email && x.Password == password);
      // return null if user not found
      if (result == null)
      {
        response.Errors.Add("Not found user with those credentials");
        return response;
      }

      var user = _mapper.Map<Domain.DataTransferObjects.Account.User>(result);

      // authentication successful so generate jwt token
      var tokenHandler = new JwtSecurityTokenHandler();

      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
          {
                    new Claim(ClaimTypes.Name, result.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.RoleName)
          }),
        Expires = DateTime.UtcNow.AddHours(12),
        SigningCredentials = new SigningCredentials(
          new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      user.Token = tokenHandler.WriteToken(token);

      response.Data = user;
      return response;
    }

    public async Task<Result<Domain.DataTransferObjects.Account.User>> Register(UserSignup model)
    {
      var response = new Result<Domain.DataTransferObjects.Account.User>();
      var user = _mapper.Map<Domain.Entities.Account.User>(model);
      var userByEmail = Get(x => x.Email == model.Email);
      if (userByEmail != null)
      {
        response.Errors.Add($"The email {model.Email} already exists");
        return response;
      }
      var data = await Create(user);
      if (!data.Status)
      {
        response.Errors = data.Errors;
        return response;
      }
      return await Authenticate(model.Email, model.Password);
    }

    public async Task<Result<Domain.DataTransferObjects.Account.User>> GetData(Guid userId)
    {
      var response = new Result<Domain.DataTransferObjects.Account.User>();
      var result = await Queryable().Include("Role").FirstOrDefaultAsync(x => x.Id == userId);
      var user = _mapper.Map<Domain.DataTransferObjects.Account.User>(result);
      if (user != null)
        response.Data = user;
      else
        response.Errors.Add("Not found user with those credentials");
      return response;
    }
  }
}