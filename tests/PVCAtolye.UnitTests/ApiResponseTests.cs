using FluentAssertions;
using PVCAtolye.Application.Common.Models;

namespace PVCAtolye.UnitTests;

public sealed class ApiResponseTests
{
    [Fact]
    public void OkShouldCreateSuccessfulResponse()
    {
        var response = ApiResponse.Ok("hazir");

        response.Success.Should().BeTrue();
        response.Data.Should().Be("hazir");
        response.Errors.Should().BeEmpty();
    }
}
