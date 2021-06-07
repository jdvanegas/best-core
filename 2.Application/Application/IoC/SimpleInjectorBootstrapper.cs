using Application.Contracts;
using Application.Contracts.Account;
using Application.Services;
using Application.Services.Account;
using Domain.Entities;
using SimpleInjector;

namespace Application.IoC
{
  public static class SimpleInjectorBootstrapper
  {
    public static void RegisterServices(this Container container)
    {
      container.Register<IUserService, UserService>();
    }

    public static void RegisterServicesScoped(this Container container)
    {
      container.Register<IUserService, UserService>(Lifestyle.Scoped);
    }
  }
}