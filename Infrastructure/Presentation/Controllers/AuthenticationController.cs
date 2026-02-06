using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
