using Shared.Dtos.IdentityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IAuthenticationService
    {
        public Task<UserResultDto> LoginAsync(LoginDto loginDto);

        public Task<UserResultDto> RegisterAsync(RegisterDto registerDto);
    }
}
