namespace ProjetoTerra.Application.GitHub.ViewModels;

public class WebhookViewModel
{
    public int WebhookId { get;  set; }
    public string Name { get;  set; }
    public string Url { get;  set; }
    public bool Active { get; set; }
    public IReadOnlyList<string>  Events { get; set; }
    public IReadOnlyDictionary<string, string> Configs { get; set; }
}