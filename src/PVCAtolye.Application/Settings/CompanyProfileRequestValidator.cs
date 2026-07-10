using FluentValidation;

namespace PVCAtolye.Application.Settings;

public sealed class CompanyProfileRequestValidator : AbstractValidator<CompanyProfileRequest>
{
    public CompanyProfileRequestValidator()
    {
        RuleFor(request => request.CompanyName).NotEmpty().MaximumLength(200);
        RuleFor(request => request.TaxOffice).MaximumLength(128);
        RuleFor(request => request.TaxNumber).MaximumLength(32);
        RuleFor(request => request.Address).MaximumLength(512);
        RuleFor(request => request.Phone).MaximumLength(32);
        RuleFor(request => request.Email).EmailAddress().MaximumLength(128).When(request => !string.IsNullOrWhiteSpace(request.Email));
        RuleFor(request => request.CurrencyCode).NotEmpty().Length(3);
        RuleFor(request => request.DefaultVatRate).InclusiveBetween(0, 100);
        RuleFor(request => request.QuoteFooterNote).MaximumLength(1024);
    }
}
