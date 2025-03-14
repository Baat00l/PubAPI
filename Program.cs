using Microsoft.EntityFrameworkCore;
using PublisherData;
using PublisherDomain;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// L�gg till tj�nster f�r DbContext och konfigurera databasanslutning
builder.Services.AddDbContext<PubContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("PubConnection"))
           .EnableSensitiveDataLogging()
           .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

// Hantera cykliska referenser vid JSON-serialisering
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// L�gg till Swagger f�r dokumentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Initiera databasen med testdata
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PubContext>();
    DbInitializer.Initialize(dbContext);  // L�gg till testdata
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// API-endpoints f�r Authors
app.MapGet("/api/author", async (PubContext db) =>
{
    return await db.Authors.Include(a => a.Books).AsNoTracking().ToListAsync();
});

app.MapGet("/api/author/{id}", async (int id, PubContext db) =>
{
    var author = await db.Authors
        .Include(a => a.Books) // Inkludera relaterade b�cker
        .AsNoTracking()
        .FirstOrDefaultAsync(a => a.AuthorId == id);

    return author is not null ? Results.Ok(author) : Results.NotFound();
});

app.MapPost("/api/author", async (Author author, PubContext db) =>
{
    db.Authors.Add(author);
    await db.SaveChangesAsync();
    return Results.Created($"/api/author/{author.AuthorId}", author);
});

app.MapPut("/api/author/{id}", async (int id, Author updatedAuthor, PubContext db) =>
{
    var author = await db.Authors.FindAsync(id);
    if (author is null) return Results.NotFound();

    author.FirstName = updatedAuthor.FirstName;
    author.LastName = updatedAuthor.LastName;
    await db.SaveChangesAsync();
    return Results.Ok(author);
});

app.MapDelete("/api/author/{id}", async (int id, PubContext db) =>
{
    var author = await db.Authors.FindAsync(id);
    if (author is null) return Results.NotFound();

    db.Authors.Remove(author);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();
