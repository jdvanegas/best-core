using AutoMapper;
using Domain.Common;
using System.Linq;

namespace Domain.EntityMapper
{
  public class MicroShopsProfile : Profile
  {
    public MicroShopsProfile()
    {
      CreateMap<Entities.Account.User, DataTransferObjects.Account.User>();
      CreateMap<Entities.Account.User, DataTransferObjects.Account.User>().ReverseMap();

      CreateMap<Entities.Account.User, DataTransferObjects.Account.UserSignup>().ReverseMap()
        .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 3))
        .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password.HashPassword()));
    }
  }
}