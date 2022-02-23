using ECommerce.Models.Request.Users;
using FluentValidation;

namespace ECommerce.Models.Request.Validations
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .MaximumLength(200).WithMessage("First name can not over 200 characters");

            RuleFor(x => x.LastName)
                .MaximumLength(200).WithMessage("Last name can not over 200 characters");

            RuleFor(x => x.DateOfBirth)
                .GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Birthday cannot over 100 olds");

            RuleFor(x => x.Email)
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage("Email format not match");

            RuleFor(x => x.UserName)
                .MaximumLength(200).WithMessage("user name cannot over 200 characters");

            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("Password at least 6 characters");

            RuleFor(x => x).Custom((request, context) =>
                {
                    if (request.Password != request.ConfirmPassword)
                    {
                        context.AddFailure("Confirm password is not match");
                    }
                });
        }
    }
}