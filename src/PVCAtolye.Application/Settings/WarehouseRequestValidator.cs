using FluentValidation;

namespace PVCAtolye.Application.Settings;

public sealed class WarehouseRequestValidator : AbstractValidator<WarehouseRequest>
{
    public WarehouseRequestValidator()
    {
        RuleFor(request => request.Code).NotEmpty().MaximumLength(64);
        RuleFor(request => request.Name).NotEmpty().MaximumLength(128);
        RuleFor(request => request.Address).MaximumLength(512);
    }
}
