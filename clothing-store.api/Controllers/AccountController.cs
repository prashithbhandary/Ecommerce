using clothing_store.application.Dtos;
using clothing_store.application.interfaces;
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

      return Ok("Profile updated successfully");
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAccount(string email)
    {
      var success = await _accountService.DeleteAccountAsync(email);
      if (!success) return BadRequest("Failed to delete account");

      return Ok("Account deleted successfully");
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(ResetPasswordDto resetDto)
    {
      var success = await _accountService.ResetPasswordAsync(resetDto);
      if (!success) return BadRequest("Failed to reset password");

      return Ok("Password reset successfully");
    }

    [HttpPost("add-address")]
    public async Task<IActionResult> AddAddress(AddAddressDto dto)
    {
      var success = await _accountService.AddAddressAsync(dto);
      if (!success) return BadRequest("Failed to add address");
      return Ok("Address added successfully");
    }

    [HttpPut("update-address")]
    public async Task<IActionResult> UpdateAddress(UpdateAddressDto dto)
    {
      var success = await _accountService.UpdateAddressAsync(dto);
      if (!success) return BadRequest("Failed to update address");
      return Ok("Address updated successfully");
    }


  }
}
