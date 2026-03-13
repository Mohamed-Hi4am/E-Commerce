using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;
using Shared.Dtos.OrderModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager serviceManager) : ApiControllerBase
    {
        [HttpPost("Login")]    // POST : BaseURL/API/Authentication/Login
        public async Task<ActionResult<UserResultDto>> Login(LoginDto loginDto)
            => Ok(await serviceManager.AuthenticationService.LoginAsync(loginDto));

        [HttpPost("Register")] // POST : BaseURL/API/Authentication/Register
        public async Task<ActionResult<UserResultDto>> Register(RegisterDto registerDto)
            => Ok(await serviceManager.AuthenticationService.RegisterAsync(registerDto));

        [HttpGet("EmailExists")] // GET: BaseURL/API/Authentication/EmailExists
        public async Task<ActionResult<bool>> CheckEmailExist(string email)
            => Ok(await serviceManager.AuthenticationService.CheckEmailExist(email));

        [Authorize]
        [HttpGet] // GET: BaseURL/API/Authentication
        public async Task<ActionResult<UserResultDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var result = await serviceManager.AuthenticationService.
            GetUserByEmail(email!);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("Address")] // GET: BaseURL/API/Authentication/Address
        public async Task<ActionResult<AddressDto>> GetAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var result = await serviceManager.AuthenticationService.
            GetUserAddress(email!);

            return Ok(result);
        }

        [Authorize]
        [HttpPut("Address")] // PUT: BaseURL/API/Authentication/Address
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var result = await serviceManager.AuthenticationService.
            UpdateUserAddress(address, email!);

            return Ok(result);
        }
    }
}
