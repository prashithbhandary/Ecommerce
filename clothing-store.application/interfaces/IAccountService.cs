using clothing_store.application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.interfaces
{
  public interface IAccountService
  {
    Task<bool> RegisterAsync(RegisterUserDto registerUserDto);
    Task<string?> LoginAsync(LoginUserDto loginUserDto);
    Task<bool> UpdateAsync(UpdateUserDto userDto);
    Task<bool> DeleteAccountAsync(string email);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetDto);
    Task<bool> AddAddressAsync(AddAddressDto dto);
    Task<bool> UpdateAddressAsync(UpdateAddressDto dto);

  }
}
