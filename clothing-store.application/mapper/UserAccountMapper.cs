using clothing_store.application.Dtos;
using clothing_store.application.mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.infrastructure.mapper
{
  public class UserAccountMapper : IUserAccountMapper
  {
    public ApplicationUser MapUpdateDtoToUser(UpdateUserDto dto, ApplicationUser user)
    {
      user.FullName = dto.FullName;
      user.PhoneNumber = dto.PhoneNumber;
      user.Email = dto.Email;
      return user;
    }
      public Address MapAddDtoToAddress(AddAddressDto dto)
      {
      var address = new Address { };
      address.addressLine1 = dto.AddressLine1;
      address.addressLine2 = dto.AddressLine2;
      address.City = dto.City;
      address.State = dto.State;
      address.PostalCode = dto.PostalCode;
      address.Country = dto.Country;
      address.UserId = dto.UserId;
      return address;
      }

      public Address MapUpdateDtoToAddress(UpdateAddressDto dto)
      {
      var address = new Address { };
      address.addressLine1 = dto.AddressLine1;
        address.addressLine2 = dto.AddressLine2;
        address.City = dto.City;
        address.State = dto.State;
        address.PostalCode = dto.PostalCode;
        address.Country = dto.Country;
      return address;
    }
    }

  }
