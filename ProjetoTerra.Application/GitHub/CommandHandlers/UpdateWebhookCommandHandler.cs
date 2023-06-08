using MediatR;
using Octokit;
using ProjetoTerra.Application.GitHub.Commands;
using ProjetoTerra.Shared.Exceptions;
using ProjetoTerra.Shared.Helpers;

namespace ProjetoTerra.Application.GitHub.CommandHandlers;

public class UpdateWebhookCommandHandler : IRequestHandler<UpdateWebhookCommand, bool>
{
    private readonly GitHubClient _githubClient;

    public UpdateWebhookCommandHandler(GitHubClient githubClient)
    {
        _githubClient = githubClient;
    }
    
    public async Task<bool> Handle(UpdateWebhookCommand request, CancellationToken cancellationToken)
    {
        var repositories = await _githubClient.Repository.GetAllForCurrent();
        var repositoryOwnerName = repositories.FirstOrDefault(r => r.Name.Equals(request.RepositoryName))?.Owner.Login;
        
        if (string.IsNullOrEmpty(repositoryOwnerName))
        {
            throw new InvalidActionException(ResourceHelper.RepositoryNotFound);
        }
        
        var hookUpdate = new EditRepositoryHook(request.Config)
        {
            Active = request.Active,
            Events = request.Events
        };

        try
        {
            var hook = await _githubClient.Repository.Hooks.Edit(repositoryOwnerName, request.RepositoryName, request.WebhookId, hookUpdate);
        
            if (hook == null)
            {
                throw new InvalidActionException(ResourceHelper.UpdateHookFailed);
            }
        }
        catch (Exception)
        {
            throw new InvalidActionException(ResourceHelper.UpdateHookFailed);
        }

        return true;
    }
}