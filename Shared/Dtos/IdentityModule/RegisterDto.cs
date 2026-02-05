using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.IdentityModule
{
    public record RegisterDto
    {
        [Required(ErrorMessage = "Display Name is Required !!")]
        public string DisplayName { get; init; }

        [Required(ErrorMessage = "User Name is Required !!")]
        public string UserName { get; init; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is Required !!")]
        public string Email { get; init; }

        public string Password { get; init; }

        public string? PhoneNumber { get; init; }
    }
}
