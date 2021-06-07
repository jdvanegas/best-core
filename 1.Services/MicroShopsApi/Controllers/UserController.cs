using Application.Contracts.Account;
using AutoMapper;
using Domain.Common;
using Domain.DataTransferObjects.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroShopsApi.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly ILogger _logger;
    private readonly IUserService _service;

    public UserController(ILogger logger, IUserService service)
    {
      _logger = logger;
      _service = service;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserSignin model)
    {
      _logger.LogInformation($"POST Login user: {model.Email} called");
      var result = await _service.Authenticate(model.Email, model.Password);

      if (!result.Status)
        return BadRequest(result);

      return Ok(result);
    }

    [HttpGet(""), Authorize]
    public async Task<IActionResult> Get()
    {
      _logger.LogInformation($"GET User by Id: {User.Identity.Name} called");
      var data = await _service.GetData(Guid.Parse(User.Identity.Name));
      if (data == null)
      {
        _logger.LogInformation($"GET User by Id: {User.Identity.Name} NOT FOUND");
        return NotFound(data);
      }
      return Ok(data);
    }

    [HttpPost("")]
    public async Task<IActionResult> Create(UserSignup user)
    {
      _logger.LogInformation($"POST User create called");
      var result = await _service.Register(user);
      if (!result.Status) return BadRequest(result);
      return base.Created("", result);
    }
  }
}