using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogoFilmes.ApiEndPoints
{
    public static class GeneroEndpoints
    {
        public static void MapGeneroEndpoints(this WebApplication app)
        {
            app.MapGet("/genero", async (AppDbContext db) => await db.Generos.ToListAsync());

            app.MapGet("/genero/{id:int}", async (int id, AppDbContext db) =>
            {
                return await db.Generos.FindAsync(id) is Genero genero
                    ? Results.Ok(genero)
                    : Results.NotFound($"Genero com id = {id} não encontrado...");
            });

            app.MapGet("/genero/filme", async (AppDbContext db) =>
            {
                var tudo = db.Generos.Include(f => f.Filmes).ToList();
                if (tudo is null) return Results.NotFound();
                return Results.Ok(tudo);
            });

            app.MapPost("/genero", async (Genero genero, AppDbContext db) =>
            {
                if (genero is null)
                {
                    return Results.BadRequest("Dados incorretos...");
                }
                db.Add(genero);
                await db.SaveChangesAsync();
                return Results.Created($"/genero/{genero.GeneroId}", genero);
            });

            app.MapPut("/genero/{id:int}", async (int id, Genero generoUpdate, AppDbContext db) =>
            {
                if (generoUpdate.GeneroId != id) return Results.BadRequest();

                var genero = await db.Generos.FindAsync(id);

                if (genero is null) return Results.NotFound();

                genero.Nome = generoUpdate.Nome;
                await db.SaveChangesAsync();

                return Results.Ok(genero);
            });

            app.MapDelete("/genero/{id:int}", async (int id, AppDbContext db) =>
            {
                var genero = await db.Generos.FindAsync(id);

                if (genero is null) return Results.NotFound();

                db.Generos.Remove(genero);
                await db.SaveChangesAsync();

                return Results.Ok(genero);
            });


        }
    }
}