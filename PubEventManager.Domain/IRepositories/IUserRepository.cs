using PubEventManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Domain.IRepositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task AddAsync(User user);
    Task<User> GetUserByEmailAsync(string email);

}
