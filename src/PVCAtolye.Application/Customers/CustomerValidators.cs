using FluentValidation;
using PVCAtolye.Domain.Customers;

namespace PVCAtolye.Application.Customers;

public sealed class CustomerAddressRequestValidator : AbstractValidator<CustomerAddressRequest>
{
    public CustomerAddressRequestValidator()
    {
        RuleFor(request => request.Title).NotEmpty().MaximumLength(80);
        RuleFor(request => request.AddressLine).NotEmpty().MaximumLength(500);
        RuleFor(request => request.District).MaximumLength(100);
        RuleFor(request => request.City).MaximumLength(100);
        RuleFor(request => request.PostalCode).MaximumLength(20);
    }
}

public sealed class CustomerContactRequestValidator : AbstractValidator<CustomerContactRequest>
{
    public CustomerContactRequestValidator()
    {
        RuleFor(request => request.FullName).NotEmpty().MaximumLength(150);
        RuleFor(request => request.Title).MaximumLength(100);
        RuleFor(request => request.Phone).MaximumLength(50);
        RuleFor(request => request.Email).EmailAddress().MaximumLength(150).When(request => !string.IsNullOrWhiteSpace(request.Email));
    }
}

public sealed class CustomerRequestValidator : AbstractValidator<CustomerRequest>
{
    public CustomerRequestValidator()
    {
        RuleFor(request => request.Type).IsInEnum();
        RuleFor(request => request.DisplayName).NotEmpty().MaximumLength(200);
        RuleFor(request => request.TaxOffice).MaximumLength(100);
        RuleFor(request => request.TaxNumber).MaximumLength(50);
        RuleFor(request => request.IdentityNumber).MaximumLength(20);
        RuleFor(request => request.Phone).MaximumLength(50);
        RuleFor(request => request.Email).EmailAddress().MaximumLength(150).When(request => !string.IsNullOrWhiteSpace(request.Email));
        RuleFor(request => request.Notes).MaximumLength(1000);
        RuleFor(request => request.TaxNumber)
            .NotEmpty()
            .When(request => request.Type == CustomerType.Corporate)
            .WithMessage("Kurumsal musteri icin vergi no zorunlu.");
        RuleFor(request => request.IdentityNumber)
            .NotEmpty()
            .When(request => request.Type == CustomerType.Individual)
            .WithMessage("Bireysel musteri icin kimlik no zorunlu.");
        RuleForEach(request => request.Addresses).SetValidator(new CustomerAddressRequestValidator());
        RuleForEach(request => request.Contacts).SetValidator(new CustomerContactRequestValidator());
    }
}
