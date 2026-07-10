using PVCAtolye.Domain.Common;

namespace PVCAtolye.Domain.Audit;

public sealed class AuditLog : BaseEntity
{
    private AuditLog()
    {
    }

    public AuditLog(Guid? userId, string action, string entityName, string? entityId, DateTimeOffset occurredAt, string? details)
    {
        UserId = userId;
        Action = action;
        EntityName = entityName;
        EntityId = entityId;
        OccurredAt = occurredAt;
        Details = details;
    }

    public Guid? UserId { get; private set; }
    public string Action { get; private set; } = string.Empty;
    public string EntityName { get; private set; } = string.Empty;
    public string? EntityId { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }
    public string? Details { get; private set; }
}
