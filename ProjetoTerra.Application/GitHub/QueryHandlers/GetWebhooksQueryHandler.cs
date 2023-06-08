using MediatR;
using Octokit;
using ProjetoTerra.Application.GitHub.Queries;
using ProjetoTerra.Application.GitHub.ViewModels;
using ProjetoTerra.Shared.Exceptions;
using ProjetoTerra.Shared.Helpers;

namespace ProjetoTerra.Application.GitHub.QueryHandlers;

public class GetWebhooksQueryHandler : IRequestHandler<GetWebhooksQuery, List<WebhookViewModel>?>
{
    private readonly GitHubClient _githubClient;

    public GetWebhooksQueryHandler(GitHubClient githubClient)
    {
        _githubClient = githubClient;
    }
    
    public async Task<List<WebhookViewModel>?> Handle(GetWebhooksQuery request, CancellationToken cancellationToken)
    {
        // Pesquisa o repositório pelo nome
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
        
        var hooks = await _githubClient.Repository.Hooks.GetAll(repositoryId.Value, paginationOptions);

        return hooks?.Select(h => new WebhookViewModel()
        {
            Name = h.Name,
            Url = h.Url,
            Events = h.Events,
            Active = h.Active,
            Configs = h.Config,
            WebhookId = h.Id
        }).ToList();
    }
}