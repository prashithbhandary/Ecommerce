using clothing_store.application.Dtos;
using clothing_store.application.interfaces;
using clothing_store.application.mapper;
using clothing_store.infrastructure.Indentity;
using clothing_store.infrastructure.mapper;
using clothing_store.infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.services
{
  public class AccountService : IAccountService
  {
    private readonly AccountRepository _accountRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly JwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserAccountMapper _userAccountMapper;

    public AccountService(AccountRepository accountRepository, PasswordHasher passwordHasher, JwtTokenGenerator jwtTokenGenerator, IUserAccountMapper userAccountMapper)
    {
      _accountRepository = accountRepository;
      _passwordHasher = passwordHasher;
      _jwtTokenGenerator = jwtTokenGenerator;
      _userAccountMapper = userAccountMapper;
    }

    public async Task<bool> RegisterAsync(RegisterUserDto registerDto)
    {
      var existingUser = await _accountRepository.GetByEmailAsync(registerDto.Email);
      if (existingUser != null) return false; // Email already exists

      var hashedPassword = _passwordHasher.HashPassword(registerDto.Password);
      var user = new ApplicationUser
      {
        FullName = registerDto.FullName,
        Email = registerDto.Email,
        PhoneNumber = registerDto.PhoneNumber,
        PasswordHash = hashedPassword,
        IsAdmin = registerDto.IsAdmin
      };

      return await _accountRepository.CreateAsync(user);
    }

    public async Task<string?> LoginAsync(LoginUserDto loginDto)
    {
      var user = await _accountRepository.GetByEmailAsync(loginDto.Email);
      if (user == null || !_passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash))
        return null;

      return _jwtTokenGenerator.GenerateToken(user);
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
      return await _accountRepository.GetByEmailAsync(email);
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
    {
      return await _accountRepository.GetAllUsersAsync();
    }

    public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _accountRepository.GetByEmailAsync(email);
        }

        public async Task<ApplicationUser> GetUserByIdAsync(int UserId)
        {
            return await _accountRepository.GetByIdAsync(UserId);

        }

        public async Task<bool> UpdateAsync(UpdateUserDto userDto)
    {
      var user = await _accountRepository.GetByEmailAsync(userDto.Email);
      if (user == null) return false; // User not found
      user = _userAccountMapper.MapUpdateDtoToUser(userDto, user);
      return await _accountRepository.UpdateAsync(user);
    }

    public async Task<bool> DeleteAccountAsync(string email)
    {
      var user = await _accountRepository.GetByEmailAsync(email);
      if (user == null) return false; // User not found

      return await _accountRepository.DeleteAsync(user);
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetDto)
    {
      var user = await _accountRepository.GetByEmailAsync(resetDto.Email);
      if (user == null) return false; // User not found

      var hashedPassword = _passwordHasher.HashPassword(resetDto.NewPassword);
      user.PasswordHash = hashedPassword;

      return await _accountRepository.UpdateAsync(user);
    }

        public async Task<IEnumerable<Address>> GetAllAddressesAsync(int userId)
        {
            return await _accountRepository.GetAllAddressesByUserIdAsync(userId);
        }

        public async Task<Address?> GetAddressByIdAsync(int id)
        {
            return await _accountRepository.GetAddressByIdAsync(id);
        }

        public async Task<bool> DeleteAddressAsync(int id)
        {
            var address = await _accountRepository.GetAddressByIdAsync(id);
            if (address == null) return false;
            return await _accountRepository.DeleteAddressAsync(address);
        }

        public async Task<bool> AddAddressAsync(AddAddressDto dto)
        {
            var address = _userAccountMapper.MapAddDtoToAddress(dto);
            return await _accountRepository.AddAddressAsync(address);
        }

        public async Task<bool> UpdateAddressAsync(UpdateAddressDto dto)
        {
            var address = await _accountRepository.GetAddressByIdAsync(dto.Id);
            if (address == null) return false;

            address = _userAccountMapper.MapUpdateDtoToAddress(dto);
            return await _accountRepository.UpdateAddressAsync(address);
        }

    }
}
