using ManagerSystem.Api.Common;

namespace ManagerSystem.Api.Features.Projects;

public interface IProjectService
{
    Task<BaseResponse<ProjectResponse>> CreateAsync(Guid userId, CreateProjectRequest request);
    Task<BaseResponse<List<ProjectResponse>>> ListAsync(ProjectListQuery query);
    Task<BaseResponse<ProjectResponse>> GetAsync(Guid id);
    Task<BaseResponse<ProjectResponse>> UpdateAsync(Guid id, UpdateProjectRequest request);
    Task<BaseResponse<bool>> DeleteAsync(Guid id);
}
