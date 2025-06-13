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

/*
using (var scope = app.Services.CreateScope())
{
    var servicio = scope.ServiceProvider;

    var recursoService = servicio.GetRequiredService<IRecursoService>();
    var observer = servicio.GetRequiredService<IRecursoObserver>();

    recursoService.AgregarObservador(observer);

    //De aca para abajo se borra//
    //USUARIOS//
    var usuarioService = servicio.GetRequiredService<IUsuarioService>();

    UsuarioConContraseñaDTO admin = new UsuarioConContraseñaDTO
    {
        Email = "admin@prueba.com",
        Nombre = "Admin",
        Apellido = "Prueba",
        Contraseña = "Admin123!",
        FechaNacimiento = new DateTime(1990, 1, 1)
    };
    usuarioService.CrearUsuario(admin);
    UsuarioDTO usuarioCreadoAdmin = usuarioService.GetByEmail(admin.Email);
    usuarioService.ConvertirEnAdmin(usuarioCreadoAdmin.Id);

    UsuarioConContraseñaDTO admin2 = new UsuarioConContraseñaDTO
    {
        Email = "admin2@prueba.com",
        Nombre = "Admin2",
        Apellido = "Prueba",
        Contraseña = "Admin123!",
        FechaNacimiento = new DateTime(2004, 3, 17),
    };
    usuarioService.CrearUsuario(admin2);
    UsuarioDTO usuarioCreadoAdmin2 = usuarioService.GetByEmail(admin2.Email);
    usuarioService.ConvertirEnAdmin(usuarioCreadoAdmin2.Id);

    UsuarioConContraseñaDTO user1 = new UsuarioConContraseñaDTO
    {
        Email = "user@prueba.com",
        Nombre = "Usuario",
        Apellido = "Prueba",
        Contraseña = "User1234!",
        FechaNacimiento = new DateTime(1995, 5, 5)
    };
    usuarioService.CrearUsuario(user1);
    UsuarioDTO usuarioCreado1 = usuarioService.GetByEmail(user1.Email);

    UsuarioConContraseñaDTO user2 = new UsuarioConContraseñaDTO
    {
        Email = "user2@prueba.com",
        Nombre = "Usuario2",
        Apellido = "Prueba",
        Contraseña = "User1234!",
        FechaNacimiento = new DateTime(1983, 4, 9)
    };
    usuarioService.CrearUsuario(user2);
    UsuarioDTO usuarioCreado2 = usuarioService.GetByEmail(user2.Email);

    UsuarioConContraseñaDTO user3 = new UsuarioConContraseñaDTO
    {
        Email = "user3@prueba.com",
        Nombre = "Usuario3",
        Apellido = "Prueba",
        Contraseña = "User1234!",
        FechaNacimiento = new DateTime(2007, 9, 17) // Corregido el año inválido (207)
    };
    usuarioService.CrearUsuario(user3);
    UsuarioDTO usuarioCreado3 = usuarioService.GetByEmail(user3.Email);

    UsuarioConContraseñaDTO user4 = new UsuarioConContraseñaDTO
    {
        Email = "user4@prueba.com",
        Nombre = "Usuario4",
        Apellido = "Prueba",
        Contraseña = "User1234!",
        FechaNacimiento = new DateTime(2007, 9, 17)
    };
    usuarioService.CrearUsuario(user4);
    UsuarioDTO usuarioCreado4 = usuarioService.GetByEmail(user4.Email);

    UsuarioConContraseñaDTO user5 = new UsuarioConContraseñaDTO
    {
        Email = "user5@prueba.com",
        Nombre = "Usuario5",
        Apellido = "Prueba",
        Contraseña = "User1234!",
        FechaNacimiento = new DateTime(2005, 2, 24)
    };
    usuarioService.CrearUsuario(user5);
    UsuarioDTO usuarioCreado5 = usuarioService.GetByEmail(user5.Email);

    //PROYECTOS//
    var proyectoService = servicio.GetRequiredService<IProyectoService>();

    ProyectoDTO proyecto1 = new ProyectoDTO
    {
        Nombre = "Proyecto 1",
        Descripcion = "Descripcion1",
        FechaInicio = DateTime.Now.Add(new TimeSpan(3, 0, 0, 0))
    };
    ProyectoDTO proyectoCreado1 = proyectoService.CrearProyecto(proyecto1);

    ProyectoDTO proyecto2 = new ProyectoDTO
    {
        Nombre = "Proyecto 2",
        Descripcion = "Descripcion2",
        FechaInicio = DateTime.Now.Add(new TimeSpan(40, 0, 0, 0))
    };
    ProyectoDTO proyectoCreado2 = proyectoService.CrearProyecto(proyecto2);

    ProyectoDTO proyecto3 = new ProyectoDTO
    {
        Nombre = "Proyecto 3",
        Descripcion = "Descripcion3",
        FechaInicio = DateTime.Now
    };
    ProyectoDTO proyectoCreado3 = proyectoService.CrearProyecto(proyecto3);

    ProyectoDTO proyecto4 = new ProyectoDTO
    {
        Nombre = "Proyecto 4",
        Descripcion = "Descripcion4",
        FechaInicio = DateTime.Now.Add(new TimeSpan(15, 0, 0, 0))
    };
    ProyectoDTO proyectoCreado4 = proyectoService.CrearProyecto(proyecto4);

    ProyectoDTO proyecto5 = new ProyectoDTO
    {
        Nombre = "Proyecto 5",
        Descripcion = "Descripcion5",
        FechaInicio = DateTime.Now.Add(new TimeSpan(10, 0, 0, 0))
    };
    ProyectoDTO proyectoCreado5 = proyectoService.CrearProyecto(proyecto5);

    ProyectoDTO proyecto6 = new ProyectoDTO
    {
        Nombre = "Proyecto 6",
        Descripcion = "Descripcion6",
        FechaInicio = DateTime.Now.Add(new TimeSpan(31, 0, 0, 0))
    };
    ProyectoDTO proyectoCreado6 = proyectoService.CrearProyecto(proyecto6);

    //RECURSOS//
    RecursoDTO recurso1 = new RecursoDTO
    {
        Nombre = "Auto Rojo",
        Tipo = "Vehiculo",
        Descripcion = "Auto rojo electrico",
        SePuedeCompartir = false,
        CantidadDelRecurso = 1
    };
    RecursoDTO recursoCreado1 = recursoService.Add(recurso1);

    RecursoDTO recurso2 = new RecursoDTO
    {
        Nombre = "Desarrollador Frontend",
        Tipo = "Humano",
        Descripcion = "Experto en tecnologías como React, Angular y Vue.js",
        SePuedeCompartir = true,
        CantidadDelRecurso = 2
    };
    RecursoDTO recursoCreado2 = recursoService.Add(recurso2);

    RecursoDTO recurso3 = new RecursoDTO
    {
        Nombre = "Desarrollador Backend",
        Tipo = "Humano",
        Descripcion = "Especialista en Node.js, Python y bases de datos SQL/NoSQL",
        SePuedeCompartir = true,
        CantidadDelRecurso = 3
    };
    RecursoDTO recursoCreado3 = recursoService.Add(recurso3);

    RecursoDTO recurso4 = new RecursoDTO
    {
        Nombre = "Desarrollador Full Stack",
        Tipo = "Humano",
        Descripcion = "Con habilidades tanto en frontend como en backend",
        SePuedeCompartir = true,
        CantidadDelRecurso = 4
    };
    RecursoDTO recursoCreado4 = recursoService.Add(recurso4);

    RecursoDTO recurso5 = new RecursoDTO
    {
        Nombre = "Laptop de Desarrollo",
        Tipo = "Equipo Electrónico",
        Descripcion = "Computadora portátil con alto rendimiento para programación",
        SePuedeCompartir = false,
        CantidadDelRecurso = 5
    };
    RecursoDTO recursoCreado5 = recursoService.Add(recurso5);

    RecursoDTO recurso6 = new RecursoDTO
    {
        Nombre = "Servidor Dedicado",
        Tipo = "Equipo Electrónico",
        Descripcion = "Máquina potente para alojar aplicaciones y servicios web",
        SePuedeCompartir = false,
        CantidadDelRecurso = 6
    };
    RecursoDTO recursoCreado6 = recursoService.Add(recurso6);

    //ASIGNACION USUARIO-PROYECTO//
    //administradores
    proyectoService.AsignarAdminDeProyecto(usuarioCreado1.Id, proyectoCreado1.Id);
    proyectoService.AsignarAdminDeProyecto(usuarioCreado1.Id, proyectoCreado2.Id);
    proyectoService.AsignarAdminDeProyecto(usuarioCreado2.Id, proyectoCreado3.Id);
    proyectoService.AsignarAdminDeProyecto(usuarioCreado4.Id, proyectoCreado4.Id);
    proyectoService.AsignarAdminDeProyecto(usuarioCreadoAdmin.Id, proyectoCreado5.Id);

    //miembros 1
    proyectoService.AgregarMiembroProyecto(usuarioCreado2.Id, proyectoCreado1.Id);
    proyectoService.AgregarMiembroProyecto(usuarioCreado3.Id, proyectoCreado1.Id);
    proyectoService.AgregarMiembroProyecto(usuarioCreado4.Id, proyectoCreado1.Id);

    //miembros 2
    proyectoService.AgregarMiembroProyecto(usuarioCreado2.Id, proyectoCreado2.Id);
    proyectoService.AgregarMiembroProyecto(usuarioCreado3.Id, proyectoCreado2.Id);

    //miembros 3
    proyectoService.AgregarMiembroProyecto(usuarioCreadoAdmin2.Id, proyectoCreado3.Id);

    //miembros 4 - empty
    proyectoService.AgregarMiembroProyecto(usuarioCreado2.Id, proyectoCreado4.Id);
    proyectoService.AgregarMiembroProyecto(usuarioCreado3.Id, proyectoCreado4.Id);

    //miembros 5 - empty

    //miembrps 6 - empty

    //TAREAS//
    var tareaService = servicio.GetRequiredService<ITareaService>();

    // Proyecto 1
    TareaDTO tarea1 = new TareaDTO
    {
        Titulo = "Tarea 1",
        Descripcion = "Tarea 2",
        Duracion = new TimeSpan(15, 0, 0, 0),
        EsCritica = false,
        FechaInicio = DateTime.Now.Add(new TimeSpan(15, 0, 0, 0))
    };
    TareaDTO tarea1_1 = tareaService.CrearTarea(proyectoCreado1.Id, tarea1);

    // Proyecto con 2
    TareaDTO tareaUnica = new TareaDTO
    {
        Titulo = "Analizar requerimientos",
        Descripcion = "Revisión detallada de los requerimientos del proyecto.",
        Duracion = new TimeSpan(10, 0, 0, 0),
        EsCritica = false,
        FechaInicio = DateTime.Now.Add(new TimeSpan(45, 0, 0, 0))
    };
    TareaDTO tarea2_1 = tareaService.CrearTarea(proyectoCreado2.Id, tareaUnica);

    // Proyecto 3
    TareaDTO tarea2 = new TareaDTO
    {
        Titulo = "Diseñar arquitectura",
        Descripcion = "Definir la estructura del sistema y sus componentes.",
        Duracion = new TimeSpan(15, 0, 0, 0),
        EsCritica = true,
        FechaInicio = DateTime.Now.Add(new TimeSpan(7, 0, 0, 0))
    };
    TareaDTO tarea3_1 = tareaService.CrearTarea(proyectoCreado3.Id, tarea2);

    TareaDTO tarea3 = new TareaDTO
    {
        Titulo = "Desarrollar API",
        Descripcion = "Implementación del backend con endpoints necesarios.",
        Duracion = new TimeSpan(20, 0, 0, 0),
        EsCritica = true,
        FechaInicio = DateTime.Now.Add(new TimeSpan(10, 0, 0, 0))
    };
    TareaDTO tarea3_2 = tareaService.CrearTarea(proyectoCreado3.Id, tarea3);

    TareaDTO tarea4 = new TareaDTO
    {
        Titulo = "Pruebas automatizadas",
        Descripcion = "Creación y ejecución de pruebas automatizadas.",
        Duracion = new TimeSpan(8, 0, 0, 0),
        EsCritica = false,
        FechaInicio = DateTime.Now.Add(new TimeSpan(12, 0, 0, 0))
    };
    TareaDTO tarea3_3 = tareaService.CrearTarea(proyectoCreado3.Id, tarea4);

    // Proyecto 4 - empty (con usuarios sin tareas)

    // Proyecto 5 (sin usuarios con taras)
    TareaDTO tareaProyecto4_1 = new TareaDTO
    {
        Titulo = "Configuración inicial",
        Descripcion = "Definir ajustes y parámetros iniciales del sistema.",
        Duracion = new TimeSpan(12, 0, 0, 0),
        EsCritica = false,
        FechaInicio = DateTime.Now.Add(new TimeSpan(20, 0, 0, 0))
    };
    TareaDTO tarea4_1 = tareaService.CrearTarea(proyectoCreado5.Id, tareaProyecto4_1);

    TareaDTO tareaProyecto4_2 = new TareaDTO
    {
        Titulo = "Optimización de rendimiento",
        Descripcion = "Mejorar la velocidad y eficiencia del sistema.",
        Duracion = new TimeSpan(8, 0, 0, 0),
        EsCritica = true,
        FechaInicio = DateTime.Now.Add(new TimeSpan(25, 0, 0, 0))
    };
    TareaDTO tarea4_2 = tareaService.CrearTarea(proyectoCreado5.Id, tareaProyecto4_2);

    // Proyecto 6 - empty (sin tareas ni usuarios)

    // ASIGNACION RECURSO - TAREA
    var asignacionRecursoTareaService = servicio.GetRequiredService<IAsignacionRecursoTareaService>();

    // 1. Recurso solo utilizado por tareas de un único proyecto (Auto Rojo -> Proyecto 3: tarea3_1 y tarea3_2)
    asignacionRecursoTareaService.CrearAsignacionRecursoTarea(new AsignacionRecursoTareaDTO
    {
        Recurso = recursoCreado1,
        Tarea = tarea3_1,
        Cantidad = 1
    });
    asignacionRecursoTareaService.CrearAsignacionRecursoTarea(new AsignacionRecursoTareaDTO
    {
        Recurso = recursoCreado1,
        Tarea = tarea3_2,
        Cantidad = 1
    });

    // 2. Recurso no asignado a ninguna tarea (Servidor Dedicado -> recurso6) → no lo usamos

    // 3. Recurso asignado a tareas de distintos proyectos (Laptop de Desarrollo -> tareas de Proyecto 1 y Proyecto 5)
    asignacionRecursoTareaService.CrearAsignacionRecursoTarea(new AsignacionRecursoTareaDTO
    {
        Recurso = recursoCreado5,
        Tarea = tarea1_1,
        Cantidad = 2
    });
    asignacionRecursoTareaService.CrearAsignacionRecursoTarea(new AsignacionRecursoTareaDTO
    {
        Recurso = recursoCreado5,
        Tarea = tarea4_1,
        Cantidad = 3
    });

    // 4. Recurso asignado a varias tareas del mismo proyecto (Desarrollador Full Stack -> Proyecto 3)
    asignacionRecursoTareaService.CrearAsignacionRecursoTarea(new AsignacionRecursoTareaDTO
    {
        Recurso = recursoCreado4,
        Tarea = tarea3_1,
        Cantidad = 1
    });
    asignacionRecursoTareaService.CrearAsignacionRecursoTarea(new AsignacionRecursoTareaDTO
    {
        Recurso = recursoCreado4,
        Tarea = tarea3_2,
        Cantidad = 1
    });
    asignacionRecursoTareaService.CrearAsignacionRecursoTarea(new AsignacionRecursoTareaDTO
    {
        Recurso = recursoCreado4,
        Tarea = tarea3_3,
        Cantidad = 2
    });

    // 5. Recurso usado en una tarea de un proyecto y también en otras de otros proyectos (Desarrollador Backend)
    asignacionRecursoTareaService.CrearAsignacionRecursoTarea(new AsignacionRecursoTareaDTO
    {
        Recurso = recursoCreado3,
        Tarea = tarea3_2,
        Cantidad = 1
    });
    asignacionRecursoTareaService.CrearAsignacionRecursoTarea(new AsignacionRecursoTareaDTO
    {
        Recurso = recursoCreado3,
        Tarea = tarea2_1,
        Cantidad = 2
    });

    //Asignacion a Tareas
    tareaService.AgregarUsuario(tarea1_1.Id, usuarioCreado1.Id);
    tareaService.AgregarUsuario(tarea1_1.Id, usuarioCreado2.Id);

    tareaService.AgregarUsuario(tarea2_1.Id, usuarioCreado3.Id);

    tareaService.AgregarUsuario(tarea3_1.Id, usuarioCreado4.Id);
    tareaService.AgregarUsuario(tarea3_1.Id, usuarioCreado5.Id);
    tareaService.AgregarUsuario(tarea3_2.Id, usuarioCreado4.Id);
    tareaService.AgregarUsuario(tarea3_3.Id, usuarioCreado5.Id);

    tareaService.AgregarUsuario(tarea4_2.Id, usuarioCreado5.Id);

    //Asignacion de dependendencia de tareas
    tareaService.AgregarDependencia(tarea3_3.Id, tarea3_1.Id);
    tareaService.AgregarDependencia(tarea3_3.Id, tarea3_2.Id);

    tareaService.AgregarDependencia(tarea4_2.Id, tarea4_1.Id);
}
*/

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();