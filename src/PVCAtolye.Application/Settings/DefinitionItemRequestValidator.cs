using FluentValidation;

namespace PVCAtolye.Application.Settings;

public sealed class DefinitionItemRequestValidator : AbstractValidator<DefinitionItemRequest>
{
    public DefinitionItemRequestValidator()
    {
        RuleFor(request => request.Code).NotEmpty().MaximumLength(64);
        RuleFor(request => request.Name).NotEmpty().MaximumLength(128);
        RuleFor(request => request.Description).MaximumLength(512);
        RuleFor(request => request.SortOrder).GreaterThanOrEqualTo(0);
    }
}
