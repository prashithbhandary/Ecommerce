using clothing_store.application.Dtos;
using clothing_store.application.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace clothing_store.api.Controllers
{
    [ApiController]
  [Route("api/[controller]")]
  public class AccountController : ControllerBase
  {
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
      _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto registerDto)
    {
      var success = await _accountService.RegisterAsync(registerDto);
      if (!success) return BadRequest("Email already exists");

      return Ok(new { message = "User registered successfully" });
    }

        [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginDto)
    {
      var token = await _accountService.LoginAsync(loginDto);
      if (token == null) return Unauthorized("Invalid credentials");

      return Ok(new { Token = token });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateProfile(UpdateUserDto updateDto)
    {
      var success = await _accountService.UpdateAsync(updateDto);
      if (!success) return BadRequest("Failed to update profile");

      return Ok(new { message = "Profile updated successfully" });
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAccount(string email)
    {
      var success = await _accountService.DeleteAccountAsync(email);
      if (!success) return BadRequest("Failed to delete account");

      return Ok(new { message = "Account deleted successfully" });
    }

    [HttpGet("user-by-email")]
    public async Task<IActionResult> GetUserByEmail(string email)
        {
            var users = await _accountService.GetUserByEmailAsync(email);
            return Ok(users);
        }

        [HttpGet("user-by-id/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var users = await _accountService.GetUserByIdAsync(id);
            return Ok(users);
        }

        [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetDto)
    {
      var success = await _accountService.ResetPasswordAsync(resetDto);
      if (!success) return BadRequest("Failed to reset password");

      return Ok("Password reset successfully");
    }

        [HttpPost("admin-create-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUserByAdmin(RegisterUserDto registerDto)
        {
            var success = await _accountService.RegisterAsync(registerDto);
            if (!success) return BadRequest("Email already exists");

            return Ok(new { message = "User created successfully by Admin" });
        }

        [HttpGet("all-user")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _accountService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpDelete("delete-user/{email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var result = await _accountService.DeleteAccountAsync(email);
            if (!result) return BadRequest("Failed to delete user");
            return Ok(new { message = "User deleted successfully" });
        }

        [HttpPost("add-address")]
        public async Task<IActionResult> AddAddress(AddAddressDto dto)
        {
            var result = await _accountService.AddAddressAsync(dto);
            return result ? Ok(new { message = "Address added successfully" }) : BadRequest("Failed to add address");
        }

        [HttpPut("update-address")]
        public async Task<IActionResult> UpdateAddress(UpdateAddressDto dto)
        {
            var result = await _accountService.UpdateAddressAsync(dto);
            return result ? Ok(new { message = "Address updated successfully" }) : NotFound("Address not found");
        }

        [HttpDelete("delete-address/{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var result = await _accountService.DeleteAddressAsync(id);
            return result ? Ok(new { message = "Address deleted successfully" }) : NotFound("Address not found");
        }

        [HttpGet("addresses")]
        public async Task<IActionResult> GetAllAddresses([FromQuery] int userId)
        {
            var addresses = await _accountService.GetAllAddressesAsync(userId);
            return Ok(addresses);
        }

        [HttpGet("address/{id}")]
        public async Task<IActionResult> GetAddressById(int id)
        {
            var address = await _accountService.GetAddressByIdAsync(id);
            return address != null ? Ok(address) : NotFound("Address not found");
        }



    }
}
