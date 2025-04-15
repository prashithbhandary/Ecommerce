using clothing_store.application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.mapper
{
  public interface IUserAccountMapper
  {
    ApplicationUser MapUpdateDtoToUser(UpdateUserDto dto, ApplicationUser user);
    Address MapAddDtoToAddress(AddAddressDto dto);
    Address MapUpdateDtoToAddress(UpdateAddressDto dto);
  }
}
