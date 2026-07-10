using PVCAtolye.Domain.Common;

namespace PVCAtolye.Domain.Settings;

public sealed class NumberSequenceSetting : AuditableEntity
{
    private NumberSequenceSetting()
    {
    }

    public NumberSequenceSetting(string documentType, string prefix, int nextNumber, int paddingLength)
    {
        DocumentType = documentType.Trim();
        Prefix = prefix.Trim();
        NextNumber = nextNumber;
        PaddingLength = paddingLength;
        IsActive = true;
    }

    public string DocumentType { get; private set; } = string.Empty;
    public string Prefix { get; private set; } = string.Empty;
    public int NextNumber { get; private set; }
    public int PaddingLength { get; private set; }
    public bool IsActive { get; private set; }

    public void Update(string prefix, int nextNumber, int paddingLength, bool isActive)
    {
        Prefix = prefix.Trim();
        NextNumber = nextNumber;
        PaddingLength = paddingLength;
        IsActive = isActive;
    }
}
