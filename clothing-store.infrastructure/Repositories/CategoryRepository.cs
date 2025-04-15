using clothing_store.domain.models;
using clothing_store.infrastructure.context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.infrastructure.Repositories
{
  public class CategoryRepository
  {
    private readonly dbContext _context;

    public CategoryRepository(dbContext context)
    {
      _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
      return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
      return await _context.Categories.FindAsync(id);
    }

    public async Task<int> CreateAsync(Category category)
    {
      _context.Categories.Add(category);
      await _context.SaveChangesAsync();
      return category.Id;
    }

    public async Task<bool> UpdateAsync(Category category)
    {
      _context.Categories.Update(category);
      return await _context.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
      var category = await _context.Categories.FindAsync(id);
      if (category == null) return false;

      _context.Categories.Remove(category);
      return await _context.SaveChangesAsync() > 0;
    }
  }
}
