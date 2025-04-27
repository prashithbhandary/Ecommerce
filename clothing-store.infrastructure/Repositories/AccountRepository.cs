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

    public async Task<ApplicationUser?> GetByIdAsync(int userId)
    {
      return await _context.ApplicationUser.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<bool> CreateAsync(ApplicationUser user)
    {
      _context.ApplicationUser.Add(user);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _context.ApplicationUser.ToListAsync();
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
            if (address.IsPrimary)
            {
                var existingPrimary = await _context.Addresses
                    .Where(a => a.UserId == address.UserId && a.IsPrimary)
                    .ToListAsync();

                foreach (var addr in existingPrimary)
                {
                    addr.IsPrimary = false;
                }
            }

            _context.Addresses.Add(address);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAddressAsync(Address address)
        {
            if (address.IsPrimary)
            {
                var existingPrimary = await _context.Addresses
                    .Where(a => a.UserId == address.UserId && a.Id != address.Id && a.IsPrimary)
                    .ToListAsync();

                foreach (var addr in existingPrimary)
                {
                    addr.IsPrimary = false;
                }
            }

            var tracked = await _context.Addresses.FindAsync(address.Id);

            if (tracked != null)
            {
                _context.Entry(tracked).CurrentValues.SetValues(address);
            }
            else
            {
                _context.Addresses.Update(address); // fallback if not already tracked
            }

            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<Address?> GetAddressByIdAsync(int id)
    {
      return await _context.Addresses.FindAsync(id);
    }

        public async Task<IEnumerable<Address>> GetAllAddressesByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<bool> DeleteAddressAsync(Address address)
        {
            _context.Addresses.Remove(address);
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
