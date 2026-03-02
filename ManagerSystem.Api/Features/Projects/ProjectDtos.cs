using ManagerSystem.Api.Enums;

namespace ManagerSystem.Api.Features.Projects;

public record CreateProjectRequest(string Name, string Description);
public record UpdateProjectRequest(string Name, string Description, ProjectStatus Status);
public record ProjectResponse(Guid Id, string Name, string Description, ProjectStatus Status, Guid CreatedById);
