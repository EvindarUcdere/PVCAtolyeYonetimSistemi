namespace PVCAtolye.Domain.Settings;

public sealed class PaymentMethod : DefinitionEntity
{
    private PaymentMethod() { }
    public PaymentMethod(string code, string name, string? description, int sortOrder) : base(code, name, description, sortOrder) { }
}
