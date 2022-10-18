using ApiCatalogoFilmes;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Conseguindo a string de conexão com o db
var stringConnection = builder.Configuration.GetConnectionString("DefaultConnection");
// Adicionando o novo context no conteiner de serviços
builder.Services.AddDbContext<AppDbContext>(op => {
    op.UseMySql(stringConnection, ServerVersion.AutoDetect(stringConnection));
});
var app = builder.Build();

app.MapGet("/genero", () => (AppDbContext db) => Results.Ok(db.Generos.Include(f => f.Filmes).ToList()));
app.MapPost("/genero", async (Genero genero, AppDbContext db)=>
{
    if (genero is null)
    {
        return Results.BadRequest("Dados incorretos...");
    }
    db.Add(genero);
    await db.SaveChangesAsync();
    return Results.Created("Genero criado com sucesso", genero);
});

app.Run();
