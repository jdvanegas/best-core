using Application.IoC;
using DataAccess;
using DataAccess.IoC;
using Domain.EntityMapper.IoC;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;

namespace CrossCutting.IoC
{
  public static class SimpleInjectorBootstrapper
  {
    public static void RegisterDependencies(this Container container)
    {
      container.Register<DbContext, MicroShopsContext>();

      container.RegisterRepository();

      container.RegisterAutomapper();

      container.RegisterServices();
    }

    public static void RegisterDependenciesScoped(this Container container)
    {
      container.Register<DbContext, MicroShopsContext>(Lifestyle.Scoped);

      container.RegisterRepositoryScoped();

      container.RegisterAutomapperScoped();

      container.RegisterServicesScoped();
    }
  }
}