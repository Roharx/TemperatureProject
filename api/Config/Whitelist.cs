namespace api.Config;

public class Whitelist
{
    public static List<string> AllowedUrls { get; } = new List<string>
    {
        "http://localhost:4200/",
        "http://localhost:5296/"
    };
}