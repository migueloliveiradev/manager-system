using ManagerSystem.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ManagerSystem.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskColumn> TaskColumns => Set<TaskColumn>();
    public DbSet<WorkTask> Tasks => Set<WorkTask>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<TaskHistory> TaskHistories => Set<TaskHistory>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Project>().HasOne(x => x.CreatedBy).WithMany().HasForeignKey(x => x.CreatedById);
        builder.Entity<WorkTask>().HasOne(x => x.Assignee).WithMany().HasForeignKey(x => x.AssigneeId);
    }
}
