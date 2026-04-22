using AutoMapper;
using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.Contracts;
using Shared;
using Shared.Dtos.IdentityModule;
using Shared.Dtos.OrderModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ValidationException = Domain.Exceptions.ValidationException;

namespace Services.Implementations
{
    public class AuthenticationService(UserManager<User> _userManager,
        IOptions<JwtOptions> options,
        IMapper mapper) : IAuthenticationService
    {

        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            // Check If There Is User Under This Email
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
                throw new UnAuthorizedException($"Email {loginDto.Email} is not Exist.");

            // Check If Password Is Correct
            var result = await _userManager.CheckPasswordAsync(user,
            loginDto.Password);

            if (!result)
                throw new UnAuthorizedException();

            // Return User & Create Token
            return new UserResultDto(user.DisplayName, user.Email!, await CreateTokenAsync(user));
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
                
            }

            // Return user & create token
            return new UserResultDto(user.DisplayName, user.Email, await CreateTokenAsync(user));
        }

        private async Task<string> CreateTokenAsync(User user)
        {
            var jwtOptions = options.Value;

            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Email, user.Email!),
            };

            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
                authClaims.Add(new Claim(ClaimTypes.Role, role));

            // 1st update
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes
            (jwtOptions.SecretKey));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 2nd update
            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: authClaims,
                expires: DateTime.UtcNow.AddDays(jwtOptions.DurationInDays),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserResultDto> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email) ??
            throw new UserNotFoundException(email);

            return new UserResultDto(
            user.DisplayName,
            user.Email!,
            await CreateTokenAsync(user));
        }

        public async Task<bool> CheckEmailExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<AddressDto> GetUserAddress(string email)
        {
            var user = await _userManager.Users.Include(u => u.Address)
                                               .FirstOrDefaultAsync(u => u.Email == email) ??
                                               throw new UserNotFoundException(email);

            return mapper.Map<AddressDto>(user.Address);
        }

        public async Task<AddressDto> UpdateUserAddress(AddressDto address, string email)
        {
            var user = await _userManager.Users.Include(u => u.Address)
                                               .FirstOrDefaultAsync(u => u.Email == email) ??
                                               throw new UserNotFoundException(email);

            // User.Address  = null ? --> Create Address
            // User.Address != null ? --> Update

            if (user.Address != null) // Update
            {
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.Street = address.Street;
                user.Address.City = address.City;
                user.Address.Country = address.Country;
            }
            else // Set Address
            {
                var useraddress = mapper.Map<UserAddress>(address);
                user.Address = useraddress;
            }
            // Update the user's Address in the Database
            await _userManager.UpdateAsync(user);

            // Return the new "AddressDto"
            return mapper.Map<AddressDto>(user.Address);
        }
    }
}
