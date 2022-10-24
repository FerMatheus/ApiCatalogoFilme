using System.Text.Json.Serialization;
using ApiCatalogoFilmes;
using ApiCatalogoFilmes.ApiEndPoints;
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
app.MapGeneroEndpoints();
app.MapFilmeEndpoints();

app.Run();
