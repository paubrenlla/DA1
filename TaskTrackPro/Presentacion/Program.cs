using Blazored.LocalStorage;
using Controllers;
using DataAccess;
using Domain;
using Domain.Observers;
using IDataAcces;
using Presentacion.Components;
using Services;
using Services.Observers;
using UserInterface.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IDataAccessUsuario, UsuarioDataAccess>();
builder.Services.AddSingleton<IDataAccessProyecto, ProyectoDataAccess>();
builder.Services.AddSingleton<IDataAccessTarea, TareaDataAccess>();
builder.Services.AddScoped<IDataAccessAsignacionProyecto, AsignacionProyectoDataAccess>();
builder.Services.AddScoped<IDataAccessRecurso, RecursoDataAccess>();
builder.Services.AddScoped<IDataAccessAsignacionRecursoTarea, AsignacionRecursoTareaDataAccess>();

builder.Services.AddScoped<IRecursoObserver, ActualizadorEstadoTareas>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<ITareaService, TareaService>();
builder.Services.AddScoped<IRecursoService, RecursoService>();
builder.Services.AddScoped<IAsignacionRecursoTareaService, AsignacionRecursoTareaService>();

builder.Services.AddScoped<UsuarioController>();
builder.Services.AddScoped<ProyectoController>();
builder.Services.AddScoped<TareaController>();
builder.Services.AddScoped<RecursoController>();
builder.Services.AddScoped<AsignacionRecursoTareaControllers>();

builder.Services.AddScoped<SessionLogic>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var servicio = scope.ServiceProvider;

    var recursoService = servicio.GetRequiredService<IRecursoService>();
    var observer = servicio.GetRequiredService<IRecursoObserver>();

    recursoService.AgregarObservador(observer);
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();