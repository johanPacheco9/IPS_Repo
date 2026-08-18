using Domain.Interfaces;
using Infrastructure.AppDbContext;
using Infrastructure.Repositories;
using IPS.Components;
using Microsoft.EntityFrameworkCore;

// Habilitar comportamiento seguro de timestamps para Npgsql / PostgreSQL
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// PostgreSQL DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                       ?? "Host=localhost;Database=ips_saas_db;Username=postgres;Password=postgres";

builder.Services.AddDbContext<MainDataContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// Repositorios de Infraestructura
builder.Services.AddScoped<IEpsRepository, EpsRepository>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IProfesionalRepository, ProfesionalRepository>();
builder.Services.AddScoped<ICitaRepository, CitaRepository>();
builder.Services.AddScoped<IAdmisionRepository, AdmisionRepository>();
builder.Services.AddScoped<IHistoriaClinicaRepository, HistoriaClinicaRepository>();

var app = builder.Build();

// Auto-crear base de datos si PostgreSQL está activo (modo desarrollo)
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<MainDataContext>();
    db.Database.EnsureCreated();
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "No se pudo conectar a PostgreSQL en el arranque. La aplicación continuará. Asegúrate de tener PostgreSQL corriendo.");
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();