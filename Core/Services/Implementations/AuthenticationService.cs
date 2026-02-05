using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = Domain.Exceptions.ValidationException;

namespace Services.Implementations
{
    internal class AuthenticationService(UserManager<User> _userManager) : IAuthenticationService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            // Check If There Is User Under This Email
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                throw new UnAuthorizedException($"Email {loginDto.Email} is not Exist.");
            // We will create this exception type below

            // Check If Password Is Correct
            var result = await _userManager.CheckPasswordAsync(user,
            loginDto.Password);
            // This function returns boolean

            if (!result)
                throw new UnAuthorizedException();

            // Return User & Create Token
            return new UserResultDto(user.DisplayName, user.Email!, "Will be Token");
        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.DisplayName,
                PhoneNumber = registerDto.PhoneNumber,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description).ToList();
                throw new ValidationException(errors);
                // We will create this exception type below
            }

            // Return user & create token
            return new UserResultDto(user.DisplayName, user.Email, "Will be Token");
        }
    }
}
