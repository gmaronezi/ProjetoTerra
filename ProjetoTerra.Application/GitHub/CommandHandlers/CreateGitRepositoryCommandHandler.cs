using MediatR;
using Octokit;
using ProjetoTerra.Application.GitHub.Commands;
using ProjetoTerra.Shared.Exceptions;
using ProjetoTerra.Shared.Helpers;

namespace ProjetoTerra.Application.GitHub.CommandHandlers;

public class CreateGitRepositoryCommandHandler : IRequestHandler<CreateGitRepositoryCommand, bool>
{
    private readonly GitHubClient _githubClient;

    public CreateGitRepositoryCommandHandler(GitHubClient githubClient)
    {
        _githubClient = githubClient;
    }
    
    public async Task<bool> Handle(CreateGitRepositoryCommand request, CancellationToken cancellationToken)
    {
        var newRepo = new NewRepository(request.RepositoryName)
        {
            Description = request.Description,
            Private = request.Private
        };

        var response = await _githubClient.Repository.Create(newRepo);

        if (response == null)
        {
            throw new InvalidActionException(ResourceHelper.CreateRepositoryFailed);
        }

        return true;
    }
}