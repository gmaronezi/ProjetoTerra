using MediatR;

namespace ProjetoTerra.Application.GitHub.Commands;

public class UpdateWebhookCommand : IRequest<bool>
{
    public int WebhookId { get; private set; }
    public string? RepositoryName { get; private set; }
    public bool Active { get; set; }
    public string[] Events { get; set; }
    public IDictionary<string, string> Config { get; set; }
    
    public UpdateWebhookCommand SetIdRepositoryName(int webhookId, string repositoryName)
    {
        WebhookId = webhookId;
        RepositoryName = repositoryName;

        return this;
    }  
    
}