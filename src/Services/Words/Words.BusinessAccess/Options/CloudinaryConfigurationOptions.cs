namespace Words.BusinessAccess.Options;

public class CloudinaryConfigurationOptions
{
    public const string CloudinarySection = "Cloudinary";
    public string CloudName { get; set; }
    public string ApiKey { get; set; }
    public string ApiSecret { get; set; }
}