namespace PVCAtolye.Application.Settings;

public sealed record NumberSequenceRequest(string Prefix, int NextNumber, int PaddingLength, bool IsActive);
