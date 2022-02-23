using ECommerce.Models.Request.Users;
using FluentValidation;

namespace ECommerce.Models.Request.Validations
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Password)
                .MinimumLength(6)
                .WithMessage("Password is at least 6 characters");
        }
    }
}