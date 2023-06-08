using MediatR;
using Octokit;
using ProjetoTerra.Application.GitHub.Queries;
using ProjetoTerra.Application.GitHub.ViewModels;
using ProjetoTerra.Shared.Exceptions;
using ProjetoTerra.Shared.Helpers;

namespace ProjetoTerra.Application.GitHub.QueryHandlers;

public class GetBranchsQueryHandler : IRequestHandler<GetBranchsQuery, List<BranchViewModel>?>
{
    private readonly GitHubClient _githubClient;

    public GetBranchsQueryHandler(GitHubClient githubClient)
    {
        _githubClient = githubClient;
    }
    
    public async Task<List<BranchViewModel>?> Handle(GetBranchsQuery request, CancellationToken cancellationToken)
    {
        // Pesquisar o repositório pelo nome
        var repositories = await _githubClient.Repository.GetAllForCurrent();
        var repositoryId = repositories.FirstOrDefault(r => r.Name.Equals(request.RepositoryName))?.Id;

        if (repositoryId == null)
        {
            throw new InvalidActionException(ResourceHelper.RepositoryNotFound);
        }

        // Parâmetros para a paginação
        var paginationOptions = new ApiOptions
        {
            PageCount = request.Page,
            PageSize = request.PageSize
        };

        var branches = await _githubClient.Repository.Branch.GetAll(repositoryId.Value, paginationOptions);

        return branches?.Select(h => new BranchViewModel
        {
            Name = h.Name,
            Protected = h.Protected
        }).ToList();
    }
}