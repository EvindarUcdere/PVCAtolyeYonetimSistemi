using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PVCAtolye.Domain.Audit;

namespace PVCAtolye.Infrastructure.Persistence.Configurations;

public sealed class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("audit_logs");
        builder.HasKey(auditLog => auditLog.Id);
        builder.Property(auditLog => auditLog.Action).HasMaxLength(128).IsRequired();
        builder.Property(auditLog => auditLog.EntityName).HasMaxLength(128).IsRequired();
        builder.Property(auditLog => auditLog.EntityId).HasMaxLength(64);
        builder.Property(auditLog => auditLog.Details).HasMaxLength(2048);
        builder.HasIndex(auditLog => auditLog.OccurredAt);
        builder.HasIndex(auditLog => auditLog.UserId);
    }
}
