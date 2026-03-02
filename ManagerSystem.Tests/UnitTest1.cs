using ManagerSystem.Api.Data;
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
        var result = await service.UpdateTaskAsync(Guid.NewGuid(), task.Id, new UpdateTaskRequest("T", "D", ManagerSystem.Api.Enums.TaskPriority.High, null, column.Id, null, 200));

        Assert.False(result.HasErrors);
        Assert.Equal(100, result.Data!.Progress);
    }
}
