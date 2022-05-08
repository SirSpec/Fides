namespace ImportScheduledJobs.Options;

public class ELKOptions
{
    public string? ElasticSearchUri { get; set; }
    public string? DefaultIndex { get; set; }
}