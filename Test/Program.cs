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

// Get all tasks
app.MapGet("/tasks", async (AppDbContext db) =>
    await db.Tasks.ToListAsync());

// Get one task by ID
app.MapGet("/tasks/{id}", async (AppDbContext db, int id) =>
    await db.Tasks.FindAsync(id)
        is TaskItem task
            ? Results.Ok(task)
            : Results.NotFound());

// Add a new task
app.MapPost("/tasks", async (AppDbContext db, TaskItem task) =>
{
    db.Tasks.Add(task);
    await db.SaveChangesAsync();
    return Results.Created($"/tasks/{task.Id}", task);
});

// ✅ Update a task (PUT)
app.MapPut("/tasks/{id}", async (AppDbContext db, int id, TaskItem updatedTask) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    task.Title = updatedTask.Title;
    task.IsComplete = updatedTask.IsComplete;

    await db.SaveChangesAsync();
    return Results.Ok(task);
});

// ✅ Delete a task (DELETE)
app.MapDelete("/tasks/{id}", async (AppDbContext db, int id) =>
{
    var task = await db.Tasks.FindAsync(id);
    if (task is null) return Results.NotFound();

    db.Tasks.Remove(task);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();
