// PublisherAPI/AuthorEndpoints.cs
using Microsoft.EntityFrameworkCore;
using PublisherDomain;

namespace PublisherAPI
{
    public static class AuthorEndpoints
    {
        // Lägg till Author
        public static void MapAuthorEndpoints(this IEndpointRouteBuilder routes)
        {
            // Skapa en Author
            routes.MapPost("/api/author", async (Author author, PubContext db) =>
            {
                db.Authors.Add(author);
                await db.SaveChangesAsync();
                return Results.Created($"/api/author/{author.AuthorId}", author);
            });

            // Hämta alla Authors
            routes.MapGet("/api/author", async (PubContext db) =>
            {
                return await db.Authors.Include(a => a.Books).AsNoTracking().ToListAsync();
            });

            // Hämta en Author efter ID
            routes.MapGet("/api/author/{id}", async (int id, PubContext db) =>
            {
                var author = await db.Authors.Include(a => a.Books).AsNoTracking().FirstOrDefaultAsync(a => a.AuthorId == id);
                return author is not null ? Results.Ok(author) : Results.NotFound();
            });

            // Uppdatera en Author
            routes.MapPut("/api/author/{id}", async (int id, Author updatedAuthor, PubContext db) =>
            {
                var author = await db.Authors.FindAsync(id);
                if (author is null) return Results.NotFound();

                author.FirstName = updatedAuthor.FirstName; // Uppdatera författarens namn
                await db.SaveChangesAsync();
                return Results.Ok(author);
            });

            // Ta bort en Author
            routes.MapDelete("/api/author/{id}", async (int id, PubContext db) =>
            {
                var author = await db.Authors.FindAsync(id);
                if (author is null) return Results.NotFound();

                db.Authors.Remove(author);
                await db.SaveChangesAsync();
                return Results.Ok();
            });
        }
    }
}
