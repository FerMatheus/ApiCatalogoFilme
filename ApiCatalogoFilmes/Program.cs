using System.Text.Json.Serialization;
using ApiCatalogoFilmes;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(op =>
{
    op.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// Conseguindo a string de conexão com o db
var stringConnection = builder.Configuration.GetConnectionString("DefaultConnection");

// Adicionando o novo context no conteiner de serviços
builder.Services.AddDbContext<AppDbContext>(op =>
{
    op.UseMySql(stringConnection, ServerVersion.AutoDetect(stringConnection));
});

var app = builder.Build();

app.MapGet("/", () => "Bem vindo à Api Catalogo de Filmes");

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


//-----------------------------------------------------Endpoint Filmes------------------------------------------------------------------------------------------


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

app.Run();
