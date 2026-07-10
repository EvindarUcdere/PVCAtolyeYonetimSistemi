using PVCAtolye.Application.Common.Models;

namespace PVCAtolye.Application.Settings;

public interface ISettingsService
{
    Task<ApiResponse<CompanyProfileResponse>> GetCompanyProfileAsync(CancellationToken cancellationToken);
    Task<ApiResponse<CompanyProfileResponse>> UpdateCompanyProfileAsync(CompanyProfileRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<IReadOnlyCollection<WarehouseResponse>>> GetWarehousesAsync(CancellationToken cancellationToken);
    Task<ApiResponse<WarehouseResponse>> CreateWarehouseAsync(WarehouseRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<WarehouseResponse>> UpdateWarehouseAsync(Guid id, WarehouseRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<IReadOnlyCollection<DefinitionItemResponse>>> GetDefinitionsAsync(string definitionType, CancellationToken cancellationToken);
    Task<ApiResponse<DefinitionItemResponse>> CreateDefinitionAsync(string definitionType, DefinitionItemRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<DefinitionItemResponse>> UpdateDefinitionAsync(string definitionType, Guid id, DefinitionItemRequest request, CancellationToken cancellationToken);
    Task<ApiResponse<IReadOnlyCollection<NumberSequenceResponse>>> GetNumberSequencesAsync(CancellationToken cancellationToken);
    Task<ApiResponse<NumberSequenceResponse>> UpdateNumberSequenceAsync(string documentType, NumberSequenceRequest request, CancellationToken cancellationToken);
}
