using clothing_store.infrastructure.context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.infrastructure.Repositories
{
  public class AccountRepository
  {
    private readonly dbContext _context;

    public AccountRepository(dbContext context)
    {
      _context = context;
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
      return await _context.ApplicationUser.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<bool> CreateAsync(ApplicationUser user)
    {
      _context.ApplicationUser.Add(user);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> UpdateAsync(ApplicationUser user)
    {
      _context.ApplicationUser.Update(user);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(ApplicationUser user)
    {
      _context.ApplicationUser.Remove(user);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> AddAddressAsync(Address address)
    {
      _context.Addresses.Add(address);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<Address?> GetAddressByIdAsync(int id)
    {
      return await _context.Addresses.FindAsync(id);
    }

    public async Task<bool> UpdateAddressAsync(Address address)
    {
      _context.Addresses.Update(address);
      return await _context.SaveChangesAsync() > 0;
    }

  }
}
