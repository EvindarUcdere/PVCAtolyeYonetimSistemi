using FluentValidation;

namespace PVCAtolye.Application.Identity;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(request => request.Username).NotEmpty().MaximumLength(64);
        RuleFor(request => request.Password).NotEmpty().MaximumLength(256);
    }
}
