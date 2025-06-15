using Blazored.LocalStorage;
using Controllers;
using DataAccess;
using Domain.Observers;
using DTOs;
using IDataAcces;
using Microsoft.EntityFrameworkCore;
using Presentacion.Components;
using Services;
using Services.Observers;
using UserInterface.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<SqlContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IDataAccessUsuario, UsuarioDataAccess>();
builder.Services.AddScoped<IDataAccessProyecto, ProyectoDataAccess>();
builder.Services.AddScoped<IDataAccessTarea, TareaDataAccess>();
builder.Services.AddScoped<IDataAccessRecurso, RecursoDataAccess>();
builder.Services.AddScoped<IDataAccessAsignacionProyecto, AsignacionProyectoDataAccess>();
builder.Services.AddScoped<IDataAccessAsignacionRecursoTarea, AsignacionRecursoTareaDataAccess>();
builder.Services.AddScoped<IDataAccessNotificacion, NotificacionDataAccess>();

builder.Services.AddScoped<IRecursoObserver, ActualizadorEstadoTareas>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<ITareaService, TareaService>();
builder.Services.AddScoped<IRecursoService, RecursoService>();
builder.Services.AddScoped<IAsignacionRecursoTareaService, AsignacionRecursoTareaService>();
builder.Services.AddScoped<INotificacionService, NotificacionService>();

builder.Services.AddScoped<IUsuarioObserver, NotificadorUsuario>();

builder.Services.AddScoped<UsuarioController>();
builder.Services.AddScoped<ProyectoController>();
builder.Services.AddScoped<TareaController>();
builder.Services.AddScoped<RecursoController>();
builder.Services.AddScoped<AsignacionRecursoTareaControllers>();
builder.Services.AddScoped<NotificacionController>();

builder.Services.AddScoped<SessionLogic>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<SqlContext>();
    ctx.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();