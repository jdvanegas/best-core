using AutoMapper;
using SimpleInjector;

namespace Domain.EntityMapper.IoC
{
  public static class SimpleInjectorBootstrapper
  {
    public static void RegisterAutomapperScoped(this Container container)
    {
      var config = new MapperConfiguration(cfg =>
      {
        cfg.AddProfile<MicroShopsProfile>();

        // ignore all unmapped properties globally
        //cfg.ForAllMaps((map, exp) => exp.ForAllOtherMembers(opt => opt.Ignore()));
      });
      container.RegisterInstance(config);
      container.Register(() => config.CreateMapper(container.GetInstance), Lifestyle.Scoped);

      //config.AssertConfigurationIsValid();
    }

    public static void RegisterAutomapper(this Container container)
    {
      var config = new MapperConfiguration(cfg =>
      {
        cfg.AddProfile<MicroShopsProfile>();

        // ignore all unmapped properties globally
        //cfg.ForAllMaps((map, exp) => exp.ForAllOtherMembers(opt => opt.Ignore()));
      });
      container.RegisterInstance(config);
      container.Register(() => config.CreateMapper(container.GetInstance));

      //config.AssertConfigurationIsValid();
    }
  }
}