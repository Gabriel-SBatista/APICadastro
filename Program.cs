using APICadastro.Context;
using APICadastro.Models;
using APICadastro.Repositories;
using APICadastro.Services;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection(nameof(MongoDBSettings)));
builder.Services.AddSingleton<MongoDbContext>();
builder.Services.AddScoped<IValidator<Usuario>, UsuarioValidator>();
builder.Services.AddScoped<IValidator<Inativacao>, InativacaoValidator>();
builder.Services.AddScoped<IValidator<Empresa>, EmpresaValidator>();
builder.Services.AddScoped<UsuarioAppServices>();
builder.Services.AddScoped<InativacaoAppServices>();
builder.Services.AddScoped<CompanyAppServices>();
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<CompanyRepository>();
builder.Services.AddScoped<InativacaoRepository>();
builder.Services.AddScoped<TipoAcessoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
