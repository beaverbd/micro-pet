namespace Common.Configs;

public class ConfigBase
{
    public DatabaseConfig Database { get; set; } = null!;
    public ServiceConfig Service { get; set; } = null!;
    public QueuesGlobalConfig? Queue { get; set; }
}

public class QueuesGlobalConfig
{
    public string Host { get; set; } = null!;
    public List<QueueConfig> Queues { get; set; } = null!;
}

public class QueueConfig
{
    public string Name { get; set; } = null!;
}
