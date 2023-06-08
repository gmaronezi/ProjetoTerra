using MediatR;

namespace ProjetoTerra.Application.GitHub.Commands;

public class CreateGitRepositoryCommand : IRequest<bool>
{
    public string? RepositoryName { get; private set; }
    public string Description { get; set; }
    public bool Private { get; set; }
    
    public CreateGitRepositoryCommand SetRepositoryName(string repositoryName)
    {
        RepositoryName = repositoryName;

        return this;
    } 
}