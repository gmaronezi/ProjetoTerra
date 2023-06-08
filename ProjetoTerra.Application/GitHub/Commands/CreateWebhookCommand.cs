using MediatR;

namespace ProjetoTerra.Application.GitHub.Commands;

public class CreateWebhookCommand : IRequest<bool>
{
    public string? RepositoryName { get; private set; }
    public string Name { get; set; }
    public string URLCallback { get; set; }
    public string Secret { get; set; }
    public string[] Events { get; set; }
    public IReadOnlyDictionary<string, string> Config { get; set; }
    
    public CreateWebhookCommand SetRepositoryName(string repositoryName)
    {
        RepositoryName = repositoryName;

        return this;
    }  
}