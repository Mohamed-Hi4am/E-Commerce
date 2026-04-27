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

        public string UserName { get; init; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is Required !!")]
        public string Email { get; init; }

        [RegularExpression("(?=^.{8,}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{\":;'?/>.<,])(?!.*\\s).*$",
        ErrorMessage = "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non-alphanumeric and at least 8 characters")]
        [Required(ErrorMessage = "Password is required !!")]
        public string Password { get; init; }

        public string? PhoneNumber { get; init; }
    }
}