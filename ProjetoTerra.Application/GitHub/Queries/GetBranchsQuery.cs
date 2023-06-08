using MediatR;

namespace ProjetoTerra.Application.GitHub.Queries;

public class GetBranchsQuery : IRequest<List<string>>
{
    public string RepositoryName { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }

    public GetBranchsQuery(string repositoryName, int page, int pageSize)
    {
        RepositoryName = repositoryName;
        Page = page;
        PageSize = pageSize;
    }
}