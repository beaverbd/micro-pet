namespace Common.Configs;

public class ConfigBase
{
    public DatabaseConfig Database { get; set; } = null!;
    public ServiceConfig Service { get; set; } = null!;
}
