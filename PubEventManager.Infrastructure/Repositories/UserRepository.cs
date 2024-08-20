using Microsoft.EntityFrameworkCore;
using PubEventManager.Domain.Entities;
using PubEventManager.Domain.IRepositories;
using PubEventManager.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{

    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        if(user != null) { await _context.Users.AddAsync(user); }
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users
                        .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
