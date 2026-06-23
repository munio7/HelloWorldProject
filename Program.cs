namespace HelloWorldProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.MapGet("/", () => "Hello World from Raspberry Pi VPS!");

            app.Run("http://0.0.0.0:5000"); // Tells the app to listen on all network interfaces
        }
    }
}
