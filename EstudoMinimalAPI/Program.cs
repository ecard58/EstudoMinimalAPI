using EstudoMinimalAPI.Data;
using EstudoMinimalAPI.Model;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

async Task<List<UsuarioModel>> GetUsuarios(AppDbContext context)
{
    return await context.Usuarios.ToListAsync();
}

app.MapGet("/usuarios", async (AppDbContext context) =>
{
    return await GetUsuarios(context);
});

app.MapGet("/usuario/{id}", async (AppDbContext context, int id) =>
{
    var usuario = await context.Usuarios.FindAsync(id);

    if(usuario == null)
    {
        return Results.NotFound("Usuário não encontrado");
    }

    return Results.Ok(usuario);
});

app.MapPost("/usuario", async (AppDbContext context, UsuarioModel usuario) =>
{
    context.Usuarios.Add(usuario);
    await context.SaveChangesAsync();
    return await GetUsuarios(context);
});

app.MapPut("/usuario", async (AppDbContext context, UsuarioModel usuario) =>
{
    var usuarioDb = await context.Usuarios.AsNoTracking().FirstOrDefaultAsync(usuarioDb => usuarioDb.Id == usuario.Id);

    if(usuarioDb  == null)
    {
        return Results.NotFound("Usuário não encontrado.");
    }
    usuarioDb.Username = usuario.Username;
    usuarioDb.Email = usuario.Email;
    usuarioDb.Nome = usuario.Nome;

    context.Update(usuario);
    await context.SaveChangesAsync();
    return Results.Ok(await GetUsuarios(context));
});

app.MapDelete("/usuario/{id}", async (AppDbContext context, int id) =>
{
    var usuarioDb = await context.Usuarios.FindAsync(id);

    if (usuarioDb == null)
    {
        return Results.NotFound("Usuário não encontrado.");
    }

    context.Usuarios.Remove(usuarioDb);
    await context.SaveChangesAsync();
    return Results.Ok(await GetUsuarios(context));
});

app.Run();


