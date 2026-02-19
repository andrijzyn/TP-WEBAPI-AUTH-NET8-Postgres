using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/todos", async (AppDbContext db) =>
                await db.TodoItems.AsNoTracking().ToListAsync())
            .RequireAuthorization();

        app.MapGet("/api/todos/{id:int}", async (int id, AppDbContext db) =>
            await db.TodoItems.FindAsync(id) is TodoItem todo
                ? Results.Ok(todo)
                : Results.NotFound())
            .RequireAuthorization();

        app.MapPost("/api/todos", async (TodoItem todo, AppDbContext db) =>
        {
            db.TodoItems.Add(todo);
            await db.SaveChangesAsync();
            return Results.Created($"/api/todos/{todo.Id}", todo);
        }).RequireAuthorization();

        app.MapPut("/api/todos/{id:int}", async (int id, TodoItem input, AppDbContext db) =>
        {
            var todo = await db.TodoItems.FindAsync(id);
            if (todo is null) return Results.NotFound();

            todo.Title = input.Title;
            todo.IsCompleted = input.IsCompleted;

            await db.SaveChangesAsync();
            return Results.NoContent();
        }).RequireAuthorization();

        app.MapDelete("/api/todos/{id:int}", async (int id, AppDbContext db) =>
        {
            var todo = await db.TodoItems.FindAsync(id);
            if (todo is null) return Results.NotFound();

            db.TodoItems.Remove(todo);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }).RequireAuthorization();
    }
}