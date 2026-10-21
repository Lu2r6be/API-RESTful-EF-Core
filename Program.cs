using BibliotecaApi.Contracts;
using BibliotecaApi.Data;
using BibliotecaApi.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Biblioteca")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();

var livros = app.MapGroup("/livros").WithTags("Livros");

livros.MapGet("/", async (BibliotecaContext db) =>
    Results.Ok(await db.Livros.AsNoTracking().OrderBy(livro => livro.Id).ToListAsync()))
    .WithSummary("Listar livros")
    .Produces<List<Livro>>();

livros.MapGet("/{id:int}", async (int id, BibliotecaContext db) =>
{
    var livro = await db.Livros.AsNoTracking().FirstOrDefaultAsync(livro => livro.Id == id);
    return livro is null ? Results.NotFound() : Results.Ok(livro);
})
.WithSummary("Consultar livro por ID")
.Produces<Livro>()
.Produces(404);

livros.MapPost("/", async (LivroRequest request, BibliotecaContext db) =>
{
    var erros = request.Validar();
    if (erros.Count > 0)
        return Results.ValidationProblem(erros);

    var livro = new Livro
    {
        Titulo = request.Titulo.Trim(),
        Autor = request.Autor.Trim(),
        AnoPublicacao = request.AnoPublicacao,
        Disponivel = request.Disponivel
    };

    await db.Livros.AddAsync(livro);
    await db.SaveChangesAsync();

    return Results.Created($"/livros/{livro.Id}", livro);
})
.WithSummary("Cadastrar livro")
.Produces<Livro>(201)
.ProducesValidationProblem();

livros.MapPut("/{id:int}", async (int id, LivroRequest request, BibliotecaContext db) =>
{
    var erros = request.Validar();
    if (erros.Count > 0)
        return Results.ValidationProblem(erros);

    var livro = await db.Livros.FindAsync(id);
    if (livro is null)
        return Results.NotFound();

    livro.Titulo = request.Titulo.Trim();
    livro.Autor = request.Autor.Trim();
    livro.AnoPublicacao = request.AnoPublicacao;
    livro.Disponivel = request.Disponivel;
    await db.SaveChangesAsync();

    return Results.Ok(livro);
})
.WithSummary("Atualizar livro")
.Produces<Livro>()
.Produces(404)
.ProducesValidationProblem();

livros.MapDelete("/{id:int}", async (int id, BibliotecaContext db) =>
{
    var livro = await db.Livros.FindAsync(id);
    if (livro is null)
        return Results.NotFound();

    db.Livros.Remove(livro);
    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithSummary("Excluir livro")
.Produces(204)
.Produces(404);

app.Run();
