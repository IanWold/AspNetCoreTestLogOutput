namespace AspNetCoreTestLogOutput.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var app = WebApplication.CreateBuilder(args).Build();

        app.MapPut("/test", (ILogger<Program> logger) =>
            logger.LogError("Test error!")
        );

        app.Run();
    }
}