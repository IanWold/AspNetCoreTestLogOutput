public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        app.MapPut("/test", (ILogger<Program> logger) =>
        {
            logger.LogError("Test error!");
        });

        app.Run();
    }
}