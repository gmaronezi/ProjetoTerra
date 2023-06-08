using MediatR;
using Octokit;
using ProjetoTerra.Application.GitHub.Commands;
using ProjetoTerra.Shared.Exceptions;
using ProjetoTerra.Shared.Helpers;

namespace ProjetoTerra.Application.GitHub.CommandHandlers;

public class CreateWebhookCommandHandler : IRequestHandler<CreateWebhookCommand, bool>
{
    private readonly GitHubClient _githubClient;

    public CreateWebhookCommandHandler(GitHubClient githubClient)
    {
        _githubClient = githubClient;
    }
    
    public async Task<bool> Handle(CreateWebhookCommand request, CancellationToken cancellationToken)
    {
        var repositories = await _githubClient.Repository.GetAllForCurrent();
        var repositoryId = repositories.FirstOrDefault(r => r.Name.Equals(request.RepositoryName))?.Id;

        if (repositoryId == null)
        {
            throw new InvalidActionException(ResourceHelper.RepositoryNotFound);
        }
        
        var webhookConfig = new NewRepositoryWebHook(request.Name, request.Config, request.URLCallback)
        {
            ContentType = WebHookContentType.Json,
            Events = request.Events,
            Active = true,
            Secret = request.Secret
        };

        var hook = await _githubClient.Repository.Hooks.Create(repositoryId.Value, webhookConfig);

        if (hook == null)
        {
            throw new InvalidActionException(ResourceHelper.CreateHookFailed);
        }

        return true;
    }
}