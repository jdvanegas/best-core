using Domain.Entities;
using Domain.Entities.Account;
using Domain.Repository;
using SimpleInjector;

namespace DataAccess.IoC
{
  public static class SimpleInjectorBootstrapper
  {
    public static void RegisterRepository(this Container container)
    {
      container.Register<IRepository<User>, Repository<User>>();
    }

    public static void RegisterRepositoryScoped(this Container container)
    {
      container.Register<IRepository<User>, Repository<User>>(Lifestyle.Scoped);
    }
  }
}