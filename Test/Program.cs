using Microsoft.EntityFrameworkCore;
using Test.Data;
using Test.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure EF Core with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));

var app = builder.Build();

// Basic test route
app.MapGet("/", () => "Default response");

// Get all tasks route, compare to Mern, quiote similar
app.MapGet("/tasks", async (AppDbContext db) =>
    await db.Tasks.ToListAsync());

// Add a new task
app.MapPost("/tasks", async (AppDbContext db, TaskItem task) =>
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{task.Id}", task);
});

app.Run();
