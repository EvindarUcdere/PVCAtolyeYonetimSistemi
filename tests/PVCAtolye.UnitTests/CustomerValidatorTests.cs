using FluentAssertions;
using PVCAtolye.Application.Customers;
using PVCAtolye.Domain.Customers;

namespace PVCAtolye.UnitTests;

public sealed class CustomerValidatorTests
{
    [Fact]
    public void CustomerValidatorShouldRejectCorporateCustomerWithoutTaxNumber()
    {
        var validator = new CustomerRequestValidator();
        var request = new CustomerRequest(CustomerType.Corporate, "ABC PVC", "Kadikoy", null, null, "555", null, null, true, [], []);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CustomerValidatorShouldRejectIndividualCustomerWithoutIdentityNumber()
    {
        var validator = new CustomerRequestValidator();
        var request = new CustomerRequest(CustomerType.Individual, "Ali Veli", null, null, null, "555", null, null, true, [], []);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }
}
