using PubEventManager.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubEventManager.Application.IServices
{
    public interface IUserService
    {
        Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<(string Token, int UserId)> AuthenticationAsync(string email, string password);
        Task<RegisterUserDto> GetByIdAsync(int id);
        Task<bool> UpdateUserAsync(int id, RegisterUserDto updatedUserDto);  // Dodata metoda

    }
}
