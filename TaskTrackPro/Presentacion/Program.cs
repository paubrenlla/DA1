using Blazored.LocalStorage;
using Controllers;
using DataAccess;
using Domain;
using IDataAcces;
using Presentacion.Components;
using Services;
using UserInterface.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<IDataAccessUsuario, UsuarioDataAccess>();
builder.Services.AddScoped<IDataAccessProyecto, ProyectoDataAccess>();
builder.Services.AddScoped<IDataAccessTarea, TareaDataAccess>();
builder.Services.AddScoped<IDataAccessAsignacionProyecto, AsignacionProyectoDataAccess>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
builder.Services.AddScoped<ITareaService, TareaService>();

builder.Services.AddScoped<UsuarioController>();
builder.Services.AddScoped<ProyectoController>();
builder.Services.AddScoped<TareaController>();

builder.Services.AddScoped<SessionLogic>();

var app = builder.Build();

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