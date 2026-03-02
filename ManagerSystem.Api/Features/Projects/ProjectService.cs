using ManagerSystem.Api.Common;
using ManagerSystem.Api.Data;
using ManagerSystem.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ManagerSystem.Api.Features.Projects;

public class ProjectService(AppDbContext db) : IProjectService
{
    public async Task<BaseResponse<ProjectResponse>> CreateAsync(Guid userId, CreateProjectRequest request)
    {
        var project = new Project { Id = Guid.NewGuid(), Name = request.Name, Description = request.Description, CreatedById = userId };
        db.Projects.Add(project);
        db.TaskColumns.AddRange(CreateDefaultColumns(project.Id));
        await db.SaveChangesAsync();
        return BaseResponse<ProjectResponse>.Success(ToDto(project));
    }

    public async Task<BaseResponse<List<ProjectResponse>>> ListAsync() => BaseResponse<List<ProjectResponse>>.Success(await db.Projects.Select(x => new ProjectResponse(x.Id, x.Name, x.Description, x.Status, x.CreatedById)).ToListAsync());

    public async Task<BaseResponse<ProjectResponse>> GetAsync(Guid id)
    {
        var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == id);
        return project is null ? BaseResponse<ProjectResponse>.Failure("Project not found.") : BaseResponse<ProjectResponse>.Success(ToDto(project));
    }

    public async Task<BaseResponse<ProjectResponse>> UpdateAsync(Guid id, UpdateProjectRequest request)
    {
        var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == id);
        if (project is null) return BaseResponse<ProjectResponse>.Failure("Project not found.");
        project.Name = request.Name;
        project.Description = request.Description;
        project.Status = request.Status;
        await db.SaveChangesAsync();
        return BaseResponse<ProjectResponse>.Success(ToDto(project));
    }

    public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
    {
        var hasTasks = await db.Tasks.AnyAsync(x => x.ProjectId == id);
        if (hasTasks) return BaseResponse<bool>.Failure("Project has dependent tasks.");
        var project = await db.Projects.FirstOrDefaultAsync(x => x.Id == id);
        if (project is null) return BaseResponse<bool>.Failure("Project not found.");
        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        return BaseResponse<bool>.Success(true);
    }

    private static ProjectResponse ToDto(Project project) => new(project.Id, project.Name, project.Description, project.Status, project.CreatedById);

    private static List<TaskColumn> CreateDefaultColumns(Guid projectId) =>
    [
        new() { ProjectId = projectId, Name = "Para Fazer", Order = 1 },
        new() { ProjectId = projectId, Name = "Fazendo", Order = 2 },
        new() { ProjectId = projectId, Name = "Feito", Order = 3 }
    ];
}
