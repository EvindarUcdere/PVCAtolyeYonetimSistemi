using FluentValidation;

namespace PVCAtolye.Application.Settings;

public sealed class NumberSequenceRequestValidator : AbstractValidator<NumberSequenceRequest>
{
    public NumberSequenceRequestValidator()
    {
        RuleFor(request => request.Prefix).NotEmpty().MaximumLength(32);
        RuleFor(request => request.NextNumber).GreaterThan(0);
        RuleFor(request => request.PaddingLength).InclusiveBetween(1, 12);
    }
}
