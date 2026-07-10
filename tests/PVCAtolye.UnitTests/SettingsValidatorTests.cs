using FluentAssertions;
using PVCAtolye.Application.Settings;

namespace PVCAtolye.UnitTests;

public sealed class SettingsValidatorTests
{
    [Fact]
    public void DefinitionItemValidatorShouldRejectEmptyCode()
    {
        var validator = new DefinitionItemRequestValidator();
        var request = new DefinitionItemRequest(string.Empty, "Metre", null, 10, true);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void CompanyProfileValidatorShouldRejectInvalidVatRate()
    {
        var validator = new CompanyProfileRequestValidator();
        var request = new CompanyProfileRequest("PVC Atolye", null, null, null, null, null, "TRY", 101m, null);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void WarehouseValidatorShouldAcceptDefaultActiveWarehouse()
    {
        var validator = new WarehouseRequestValidator();
        var request = new WarehouseRequest("MAIN", "Ana Depo", null, IsDefault: true, IsActive: true);

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
}
