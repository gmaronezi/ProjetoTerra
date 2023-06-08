using MediatR;
using Octokit;

namespace ProjetoTerra.Application.GitHub.Queries;

public class GetWebhooksQuery : IRequest<List<RepositoryHook>>
{
    public string RepositoryName { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }

    public GetWebhooksQuery(string repositoryName, int page, int pageSize)
    {
        RepositoryName = repositoryName;
        Page = page;
        PageSize = pageSize;
    }
}