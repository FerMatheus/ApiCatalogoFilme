using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoFilmes.ApiEndPoints
{
    public static class FilmeEndpoints
    {
        public static void MapFilmeEndpoints(this WebApplication app)
        {
            app.MapGet("/filme", async (AppDbContext db) => await db.Filmes.ToListAsync());

            app.MapGet("/filme/{id:int}", async (int id, AppDbContext db) =>
            {
                return await db.Filmes.FindAsync(id) is Filme filme
                    ? Results.Ok(filme)
                    : Results.NotFound();
            });

            app.MapPost("/filme", async (Filme filme, AppDbContext db) =>
            {
                if (filme != null)
                {
                    db.Add(filme);
                    await db.SaveChangesAsync();
                    return Results.Created($"/filme/{filme.FilmeId}", filme);
                }
                return Results.BadRequest("Dados incorretos...");
            });

            app.MapPut("/filme/{id:int}", async (int id, Filme filmeUpdate, AppDbContext db) =>
            {
                if (filmeUpdate.FilmeId != id) return Results.BadRequest();
                var filme = await db.Filmes.FindAsync(id);

                if (filme is null) return Results.NotFound();

                filme.Nome = filmeUpdate.Nome;
                filme.Sinopse = filmeUpdate.Sinopse;

                await db.SaveChangesAsync();

                return Results.Ok(filme);
            });

            app.MapDelete("/filme/{id:int}", async (int id, AppDbContext db) =>
            {
                var filme = await db.Filmes.FindAsync(id);
                if (filme is null) return Results.NotFound();

                db.Filmes.Remove(filme);
                await db.SaveChangesAsync();

                return Results.Ok(filme);
            });

        }
    }
}