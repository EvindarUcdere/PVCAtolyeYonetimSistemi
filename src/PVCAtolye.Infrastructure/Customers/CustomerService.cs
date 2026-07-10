using Microsoft.EntityFrameworkCore;
using PVCAtolye.Application.Common.Models;
using PVCAtolye.Application.Customers;
using PVCAtolye.Domain.Customers;
using PVCAtolye.Infrastructure.Persistence;

namespace PVCAtolye.Infrastructure.Customers;

public sealed class CustomerService(AppDbContext dbContext) : ICustomerService
{
    public async Task<ApiResponse<PagedResponse<CustomerListItemResponse>>> GetCustomersAsync(CustomerListQuery query, CancellationToken cancellationToken)
    {
        var page = Math.Max(query.Page, 1);
        var pageSize = Math.Clamp(query.PageSize, 1, 100);
        var customers = dbContext.Customers.AsNoTracking().Where(entity => !entity.IsDeleted);

        if (!query.IncludePassive)
        {
            customers = customers.Where(entity => entity.IsActive);
        }

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = $"%{query.Search.Trim()}%";
            customers = customers.Where(entity =>
                EF.Functions.ILike(entity.DisplayName, search) ||
                (entity.Phone != null && EF.Functions.ILike(entity.Phone, search)) ||
                (entity.Email != null && EF.Functions.ILike(entity.Email, search)) ||
                (entity.TaxNumber != null && EF.Functions.ILike(entity.TaxNumber, search)) ||
                (entity.IdentityNumber != null && EF.Functions.ILike(entity.IdentityNumber, search)));
        }

        var totalCount = await customers.CountAsync(cancellationToken);
        var items = await customers
            .OrderBy(entity => entity.DisplayName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(entity => new CustomerListItemResponse(entity.Id, entity.Type, entity.DisplayName, entity.Phone, entity.Email, entity.TaxNumber, entity.IsActive))
            .ToListAsync(cancellationToken);

        return ApiResponse.Ok(new PagedResponse<CustomerListItemResponse>(items, page, pageSize, totalCount));
    }

    public async Task<ApiResponse<CustomerDetailResponse>> GetCustomerAsync(Guid id, CancellationToken cancellationToken)
    {
        var customer = await GetCustomerWithChildrenAsync(id, cancellationToken);
        return customer is null
            ? ApiResponse.Fail<CustomerDetailResponse>("Musteri bulunamadi.")
            : ApiResponse.Ok(Map(customer));
    }

    public async Task<ApiResponse<CustomerDetailResponse>> CreateCustomerAsync(CustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = new Customer(request.Type, request.DisplayName, request.TaxOffice, request.TaxNumber, request.IdentityNumber, request.Phone, request.Email, request.Notes);
        customer.Update(request.Type, request.DisplayName, request.TaxOffice, request.TaxNumber, request.IdentityNumber, request.Phone, request.Email, request.Notes, request.IsActive);
        customer.ReplaceAddresses(CreateAddresses(request.Addresses));
        customer.ReplaceContacts(CreateContacts(request.Contacts));

        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok(Map(customer), "Musteri olusturuldu.");
    }

    public async Task<ApiResponse<CustomerDetailResponse>> UpdateCustomerAsync(Guid id, CustomerRequest request, CancellationToken cancellationToken)
    {
        var customer = await GetCustomerWithChildrenAsync(id, cancellationToken);
        if (customer is null)
        {
            return ApiResponse.Fail<CustomerDetailResponse>("Musteri bulunamadi.");
        }

        customer.Update(request.Type, request.DisplayName, request.TaxOffice, request.TaxNumber, request.IdentityNumber, request.Phone, request.Email, request.Notes, request.IsActive);
        customer.ReplaceAddresses(CreateAddresses(request.Addresses));
        customer.ReplaceContacts(CreateContacts(request.Contacts));
        await dbContext.SaveChangesAsync(cancellationToken);

        return ApiResponse.Ok(Map(customer), "Musteri guncellendi.");
    }

    private Task<Customer?> GetCustomerWithChildrenAsync(Guid id, CancellationToken cancellationToken)
    {
        return dbContext.Customers
            .Include(entity => entity.Addresses)
            .Include(entity => entity.Contacts)
            .SingleOrDefaultAsync(entity => entity.Id == id && !entity.IsDeleted, cancellationToken);
    }

    private static List<CustomerAddress> CreateAddresses(IReadOnlyCollection<CustomerAddressRequest> requests)
    {
        var hasDefault = false;
        var addresses = new List<CustomerAddress>();
        foreach (var request in requests)
        {
            var isDefault = request.IsDefault && !hasDefault;
            hasDefault = hasDefault || isDefault;
            addresses.Add(new CustomerAddress(request.Title, request.AddressLine, request.District, request.City, request.PostalCode, isDefault));
        }

        if (!hasDefault && addresses.Count > 0)
        {
            var first = addresses[0];
            addresses[0] = new CustomerAddress(first.Title, first.AddressLine, first.District, first.City, first.PostalCode, isDefault: true);
        }

        return addresses;
    }

    private static List<CustomerContact> CreateContacts(IReadOnlyCollection<CustomerContactRequest> requests)
    {
        var hasPrimary = false;
        var contacts = new List<CustomerContact>();
        foreach (var request in requests)
        {
            var isPrimary = request.IsPrimary && !hasPrimary;
            hasPrimary = hasPrimary || isPrimary;
            contacts.Add(new CustomerContact(request.FullName, request.Title, request.Phone, request.Email, isPrimary));
        }

        if (!hasPrimary && contacts.Count > 0)
        {
            var first = contacts[0];
            contacts[0] = new CustomerContact(first.FullName, first.Title, first.Phone, first.Email, isPrimary: true);
        }

        return contacts;
    }

    private static CustomerDetailResponse Map(Customer entity) =>
        new(
            entity.Id,
            entity.Type,
            entity.DisplayName,
            entity.TaxOffice,
            entity.TaxNumber,
            entity.IdentityNumber,
            entity.Phone,
            entity.Email,
            entity.Notes,
            entity.IsActive,
            entity.Addresses.OrderByDescending(address => address.IsDefault).ThenBy(address => address.Title).Select(Map).ToList(),
            entity.Contacts.OrderByDescending(contact => contact.IsPrimary).ThenBy(contact => contact.FullName).Select(Map).ToList());

    private static CustomerAddressResponse Map(CustomerAddress entity) =>
        new(entity.Id, entity.Title, entity.AddressLine, entity.District, entity.City, entity.PostalCode, entity.IsDefault);

    private static CustomerContactResponse Map(CustomerContact entity) =>
        new(entity.Id, entity.FullName, entity.Title, entity.Phone, entity.Email, entity.IsPrimary);
}
