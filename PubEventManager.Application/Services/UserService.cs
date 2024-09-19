using Microsoft.VisualBasic.FileIO;
using PubEventManager.Application.Dtos;
using PubEventManager.Application.IServices;
using PubEventManager.Domain.Entities;
using PubEventManager.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace PubEventManager.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;

        }

        public async Task<(string Token, int UserId)> AuthenticationAsync(string email, string password)
        {
            var user = await _userRepository.GetUserByEmailAsync(email); ;
            if (user == null || !VerifyPasswordHash(password, user.Password))
            {
                return (null,0);
            }

            var token = _tokenService.GenerateToken(user);
            return (token , user.Id);
        }

        public async Task<bool> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var user = new User
            {
                FirstName = registerUserDto.FirstName,
                LastName = registerUserDto.LastName,
                Email = registerUserDto.Email,
                Password = registerUserDto.Password,
                Type = (UserType)registerUserDto.UserType
            };
            await _userRepository.AddAsync(user);
            return true;
        }
        private bool VerifyPasswordHash(string password, string storedPassword)
        {
            if(password == storedPassword) {  return true; }
            return false;
        }
        public async Task<RegisterUserDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            return new RegisterUserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.Password,
                UserType = (int)user.Type,
            };
        }
        public async Task<bool> UpdateUserAsync(int id, RegisterUserDto updatedUserDto)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return false;

            user.FirstName = updatedUserDto.FirstName;
            user.LastName = updatedUserDto.LastName;
            user.Email = updatedUserDto.Email;
            user.Password = updatedUserDto.Password;
            user.Type = (UserType)updatedUserDto.UserType;

            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
