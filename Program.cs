using Microsoft.EntityFrameworkCore;

namespace HelloWorldProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            DotNetEnv.Env.Load();
            var builder = WebApplication.CreateBuilder(args);

            // Grab the connection string from appsettings.json
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Register our new TestDbContext to use PostgreSQL
            builder.Services.AddDbContext<TestDbContext>(options =>
                options.UseNpgsql(connectionString));

            var app = builder.Build();

            // Simple endpoint to check the API status and list database items
            app.MapGet("/todos", async (TestDbContext db) =>
            {
                // Return all items from the Postgres "Todos" table
                return await db.Todos.ToListAsync();
            });

            // Endpoint to quickly seed a test item so the DB isn't empty
            app.MapPost("/todos/seed/{todo_name}", async (string todo_name, TestDbContext db) =>
            {
                var newItem = new TodoItem { Title = todo_name, IsCompleted = false };
                db.Todos.Add(newItem);
                await db.SaveChangesAsync();
                return Results.Created($"/todos/{newItem.Id}", newItem);
            });

            app.Run();
        }
    }

// 1. Define a simple Data Model
public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    // 2. Define the Database Context
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }

        public DbSet<TodoItem> Todos => Set<TodoItem>();
    }
}
