namespace PVCAtolye.Application.Settings;

public sealed record NumberSequenceResponse(Guid Id, string DocumentType, string Prefix, int NextNumber, int PaddingLength, bool IsActive);
