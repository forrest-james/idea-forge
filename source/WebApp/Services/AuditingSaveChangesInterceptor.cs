using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace WebApp.Services
{
    public class AuditingSaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly IHttpContextAccessor _http;

        public AuditingSaveChangesInterceptor(IHttpContextAccessor http) => _http = http;

        private string? CurrentUserId => _http.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            ApplyAudit(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void ApplyAudit(DbContext? context)
        {
            if (context is null) return;

            var now = DateTime.UtcNow;
            var userId = CurrentUserId;

            foreach (var entry in context.ChangeTracker.Entries<AuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAtUtc = now;
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.LastModifiedAtUtc = null;
                    entry.Entity.LastModifiedBy = null;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModifiedAtUtc = now;
                    entry.Entity.LastModifiedBy = userId;
                }
            }
        }
    }
}