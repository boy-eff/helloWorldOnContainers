namespace Shared.Options;

public class CorsConfigurationOptions
{
    public const string CorsSection = "Cors";
    public string PolicyName { get; set; }
    public string Origin { get; set; }
}