using ManagerSystem.Api.Data;
using ManagerSystem.Api.Enums;
using ManagerSystem.Api.Features.Projects;
using ManagerSystem.Api.Features.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ManagerSystem.Tests;

public class UnitTest1
{
    private static AppDbContext BuildDbContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;
        return new AppDbContext(options);
    }

    [Fact]
    public async Task ProjectCreate_ShouldGenerateDefaultColumns()
    {
        await using var db = BuildDbContext(nameof(ProjectCreate_ShouldGenerateDefaultColumns));
        var service = new ProjectService(db);

        var result = await service.CreateAsync(Guid.NewGuid(), new CreateProjectRequest("Projeto X", "Descrição"));

        Assert.False(result.HasErrors);
        Assert.NotNull(result.Data);
        Assert.Equal(3, db.TaskColumns.Count(x => x.ProjectId == result.Data!.Id));
    }

    [Fact]
    public async Task TaskUpdate_ShouldClampProgressToOneHundred()
    {
        await using var db = BuildDbContext(nameof(TaskUpdate_ShouldClampProgressToOneHundred));
        var project = new ManagerSystem.Api.Models.Project { Id = Guid.NewGuid(), Name = "P", Description = "D", CreatedById = Guid.NewGuid() };
        var column = new ManagerSystem.Api.Models.TaskColumn { Id = Guid.NewGuid(), ProjectId = project.Id, Name = "Para fazer", Order = 1 };
        var task = new ManagerSystem.Api.Models.WorkTask { Id = Guid.NewGuid(), ProjectId = project.Id, StatusId = column.Id, Title = "T", Description = "D" };
        db.Projects.Add(project);
        db.TaskColumns.Add(column);
        db.Tasks.Add(task);
        await db.SaveChangesAsync();

        var service = new TaskService(db);
        var result = await service.UpdateTaskAsync(Guid.NewGuid(), task.Id, new UpdateTaskRequest("T", "D", TaskPriority.High, null, column.Id, null, 200));

        Assert.False(result.HasErrors);
        Assert.Equal(100, result.Data!.Progress);
    }

    [Fact]
    public async Task ProjectsList_ShouldFilterByStatus()
    {
        await using var db = BuildDbContext(nameof(ProjectsList_ShouldFilterByStatus));
        db.Projects.AddRange(
            new ManagerSystem.Api.Models.Project { Id = Guid.NewGuid(), Name = "A", Description = "Ativo", CreatedById = Guid.NewGuid(), Status = ProjectStatus.Active },
            new ManagerSystem.Api.Models.Project { Id = Guid.NewGuid(), Name = "B", Description = "Arquivado", CreatedById = Guid.NewGuid(), Status = ProjectStatus.Archived });
        await db.SaveChangesAsync();

        var service = new ProjectService(db);
        var result = await service.ListAsync(new ProjectListQuery(ProjectStatus.Archived, null));

        Assert.Single(result.Data!);
        Assert.Equal(ProjectStatus.Archived, result.Data![0].Status);
    }

    [Fact]
    public async Task TasksList_ShouldFilterByPriority()
    {
        await using var db = BuildDbContext(nameof(TasksList_ShouldFilterByPriority));
        var project = new ManagerSystem.Api.Models.Project { Id = Guid.NewGuid(), Name = "P", Description = "D", CreatedById = Guid.NewGuid() };
        var column = new ManagerSystem.Api.Models.TaskColumn { Id = Guid.NewGuid(), ProjectId = project.Id, Name = "Doing", Order = 1 };
        db.Projects.Add(project);
        db.TaskColumns.Add(column);
        db.Tasks.AddRange(
            new ManagerSystem.Api.Models.WorkTask { Id = Guid.NewGuid(), Title = "Low", Description = "D", ProjectId = project.Id, StatusId = column.Id, Priority = TaskPriority.Low },
            new ManagerSystem.Api.Models.WorkTask { Id = Guid.NewGuid(), Title = "Urgent", Description = "D", ProjectId = project.Id, StatusId = column.Id, Priority = TaskPriority.Urgent });
        await db.SaveChangesAsync();

        var service = new TaskService(db);
        var result = await service.ListTasksAsync(new TaskListQuery(project.Id, null, TaskPriority.Urgent, null, null));

        Assert.Single(result.Data!);
        Assert.Equal("Urgent", result.Data![0].Title);
    }
}
