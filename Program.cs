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
builder.Services.AddScoped<IValidator<User>, UserValidator>();
builder.Services.AddScoped<IValidator<Inactivation>, InactivationValidator>();
builder.Services.AddScoped<IValidator<Company>, CompanyValidator>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<InactivationServices>();
builder.Services.AddScoped<CompanyServices>();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<CompanyRepository>();
builder.Services.AddScoped<InactivationRepository>();
builder.Services.AddScoped<AccessTypeRepository>();
builder.Services.AddScoped<TokenServices>();

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
