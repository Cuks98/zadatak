using Microsoft.EntityFrameworkCore;
using WebApplication1;
using WebApplication1.Extensions;
using WebApplication1.Infrastructure;
using WebApplication1.Models.Settings;
using WebApplication1.Presentation.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
}

app.UseHttpsRedirection();

app.MapPost("/settings", async (AppDbContext db, CreateRequest createRequest) =>
    {
        await using var transaction = await db.Database.BeginTransactionAsync();

        var setting = new Setting
        {
            Value = createRequest.Value,
            ValidFrom = createRequest.ValidFrom
        };
        db.Settings.Add(setting);

        await db.SaveChangesAsync();
        await transaction.CommitAsync();

        return Results.Created($"/settings/{setting.Id}", setting.Id);
    })
    .WithTags("Settings")
    .WithOpenApi();

app.MapPut("/settings/{id}", async (AppDbContext db, int id, UpdateRequest updateRequest) =>
{
    var setting = await db.Settings.FindAsync(id);
    if (setting == null) return Results.NotFound();

    setting.Value = updateRequest.Value;

    await db.SaveChangesAsync();
    return Results.Ok(setting);
})
.WithTags("Settings")
.WithOpenApi();

app.MapDelete("/settings/{id}", async (AppDbContext db, int id) =>
    {
        var setting = await db.Settings.FindAsync(id);
        if (setting == null) return Results.NotFound();

        db.Settings.Remove(setting);
        await db.SaveChangesAsync();
        return Results.NoContent();
    })
    .WithTags("Settings")
    .WithOpenApi();

app.MapGet("/settings", async (AppDbContext db, DateTime? effectiveDate) =>
{
    var query = db.Settings
        .OrderByDescending(s => s.ValidFrom);

    if (effectiveDate.HasValue)
    {
        var setting = await query.FirstOrDefaultAsync(s => s.ValidFrom <= effectiveDate.Value);
        return setting != null ? Results.Ok(setting) : Results.NotFound();
    }

    var currentSetting = await query.FirstOrDefaultAsync(s => s.ValidFrom <= DateTime.UtcNow);
    return currentSetting != null ? Results.Ok(currentSetting) : Results.NotFound();
})
.WithTags("Settings")
.WithOpenApi();

app.Run();