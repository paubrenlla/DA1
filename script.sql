USE TaskTrackProDB;
GO

-- 1. Deshabilitar comprobaciones de FK para poder borrar/truncate
ALTER TABLE [TareaTarea]                   NOCHECK CONSTRAINT ALL;
ALTER TABLE [AsignacionesProyecto]         NOCHECK CONSTRAINT ALL;
ALTER TABLE [Tareas]                       NOCHECK CONSTRAINT ALL;
ALTER TABLE [Proyectos]                    NOCHECK CONSTRAINT ALL;
ALTER TABLE [Recursos]                     NOCHECK CONSTRAINT ALL;
ALTER TABLE [Usuarios]                     NOCHECK CONSTRAINT ALL;
ALTER TABLE [AsignacionesRecursoTarea]     NOCHECK CONSTRAINT ALL;
ALTER TABLE [UsuarioTarea]                 NOCHECK CONSTRAINT ALL;  -- join tarea-usuario
ALTER TABLE [NotificacionUsuarios]         NOCHECK CONSTRAINT ALL;
ALTER TABLE [NotificacionVistas]           NOCHECK CONSTRAINT ALL;
GO

-- 2. Borrar datos anteriores
DELETE FROM [TareaTarea];
DELETE FROM [AsignacionesProyecto];
DELETE FROM [AsignacionesRecursoTarea];
DELETE FROM [UsuarioTarea];
DELETE FROM [Tareas];
DELETE FROM [Proyectos];
DELETE FROM [Recursos];
DELETE FROM [NotificacionVistas];
DELETE FROM [NotificacionUsuarios];
DELETE FROM [Notificaciones];
DELETE FROM [Usuarios];
GO

-- 3. Reseed de identidades ANTES de los INSERT para que arranquen en 1
DBCC CHECKIDENT ('Usuarios',                 RESEED, 0);
DBCC CHECKIDENT ('Recursos',                 RESEED, 0);
DBCC CHECKIDENT ('Proyectos',                RESEED, 0);
DBCC CHECKIDENT ('Tareas',                   RESEED, 0);
DBCC CHECKIDENT ('AsignacionesProyecto',     RESEED, 0);
DBCC CHECKIDENT ('AsignacionesRecursoTarea', RESEED, 0);
DBCC CHECKIDENT ('Notificaciones',           RESEED, 0);
-- Las tablas de unión no tienen identidad
GO

-- 4. Insertar Usuarios
INSERT INTO [Usuarios] (EsAdminSistema, Email, Nombre, Apellido, Pwd, PwdSerializado, FechaNacimiento)
VALUES 
  (1, 'admin@ejemplo.com',    'Ada',   'Lovelace', 'Q29udHJhc2XDsWExIQ==','Q29udHJhc2XDsWExIQ==','1985-12-10T00:00:00'),
  (0, 'usuario1@ejemplo.com', 'Juan',  'Pérez',    'Q29udHJhc2XDsWExIQ==','Q29udHJhc2XDsWExIQ==','1992-05-20T00:00:00'),
  (0, 'usuario2@ejemplo.com', 'Ana',   'García',   'Q29udHJhc2XDsWExIQ==','Q29udHJhc2XDsWExIQ==','1998-11-03T00:00:00');
GO

-- 5. Insertar Recursos
INSERT INTO [Recursos] (Nombre, Tipo, Descripcion, SePuedeCompartir, CantidadDelRecurso, CantidadEnUso)
VALUES
  ('Auto Rojo',          'Vehículo',          'Auto eléctrico de la flota',           0, 1, 1),
  ('Laptop Desarrollo',  'Equipo Electrónico','Computadora para desarrolladores',     1, 5, 2),
  ('Servidor Dedicado',  'Equipo Electrónico','Servidor para despliegues',            0, 2, 0),
  ('Front-End Dev',      'Humano',            'Desarrollador especializado en UI',     1, 3, 0),
  ('Back-End Dev',       'Humano',            'Desarrollador especializado en APIs',   1, 4, 0);
GO

-- 6. Insertar Proyectos
INSERT INTO [Proyectos] (FinEstimado, Nombre, Descripcion, FechaInicio)
VALUES 
  (NULL, 'Sistema de Gestión',     'Sistema interno para gestionar recursos','2025-10-01T08:00:00'),
  (NULL, 'Plataforma Web Pública', 'Sitio web para clientes',                '2025-11-15T14:30:00');
GO

-- 7. Insertar Tareas (Duracion y Holgura en Ticks)
--    1h  =  36 000 000 000 ticks
--    2h  =  72 000 000 000 ticks
--    3h  = 108 000 000 000 ticks
--    0   =   0 ticks
INSERT INTO [Tareas] (
    EarlyStart, LateStart, EarlyFinish, LateFinish,
    Titulo, Descripcion, FechaInicio, Duracion, EsCritica,
    EstadoValor, EstadoFecha, Holgura, ProyectoId
)
VALUES
  -- Proyecto 1
  ('2025-10-01T08:00:00','2025-10-01T08:00:00', DATEADD(hour,2,'2025-10-01T08:00:00'), DATEADD(hour,2,'2025-10-01T08:00:00'),
   'Diseño Base de Datos','Crear el modelo inicial de datos','2025-10-01T08:00:00',
   72000000000, 1, 0, NULL, 0, 1),

  ('2025-10-02T09:00:00','2025-10-02T09:00:00', DATEADD(hour,3,'2025-10-02T09:00:00'), DATEADD(hour,3,'2025-10-02T09:00:00'),
   'Implementación Backend','Desarrollar API REST','2025-10-02T09:00:00',
   108000000000, 1, 0, NULL, 0, 1),

  -- Proyecto 2
  ('2025-11-15T14:30:00','2025-11-15T14:30:00', DATEADD(hour,1,'2025-11-15T14:30:00'), DATEADD(hour,1,'2025-11-15T14:30:00'),
   'Diseño UI','Prototipado con Figma','2025-11-15T14:30:00',
   36000000000, 0, 0, NULL, 0, 2),

  ('2025-11-16T10:00:00','2025-11-16T10:00:00', DATEADD(hour,2,'2025-11-16T10:00:00'), DATEADD(hour,2,'2025-11-16T10:00:00'),
   'Frontend React','Desarrollar SPA con React','2025-11-16T10:00:00',
   72000000000, 0, 0, NULL, 0, 2);
GO

-- 8. Dependencias entre Tareas
INSERT INTO [TareaTarea] (TareasDependenciaId, TareasSucesorasId)
VALUES (1,2),(3,4);
GO

-- 9. Asignaciones Proyecto-Usuario
INSERT INTO [AsignacionesProyecto] (ProyectoId, UsuarioId, Rol)
VALUES (1,2,1),(1,3,0),(2,3,0);
GO

-- 10. Asignaciones Usuario-Tarea
INSERT INTO [UsuarioTarea] (TareasAsignadasId, UsuariosAsignadosId)
VALUES (1,2),(2,3);
GO

-- 11. Asignaciones Recurso-Tarea
INSERT INTO [AsignacionesRecursoTarea] (RecursoId, TareaId, CantidadNecesaria)
VALUES (1,1,1),(2,4,2);
GO

-- 12. Insertar Notificaciones y vincularlas (todas sin vista)
INSERT INTO [Notificaciones] (Mensaje)
VALUES
  ('¡Bienvenido al sistema, Ada!'),
  ('¡Bienvenido al sistema, Juan!'),
  ('¡Bienvenido al sistema, Ana!');
GO

INSERT INTO [NotificacionUsuarios] (NotificacionesRecibidasId, UsuariosNotificadosId)
VALUES (1,1),(2,2),(3,3);
GO

-- 13. Reseed final de identidades
DBCC CHECKIDENT ('Usuarios',                 RESEED, 3);
DBCC CHECKIDENT ('Recursos',                 RESEED, 5);
DBCC CHECKIDENT ('Proyectos',                RESEED, 2);
DBCC CHECKIDENT ('Tareas',                   RESEED, 4);
DBCC CHECKIDENT ('AsignacionesProyecto',     RESEED, 3);
DBCC CHECKIDENT ('AsignacionesRecursoTarea', RESEED, 2);
DBCC CHECKIDENT ('Notificaciones',           RESEED, 3);
GO

-- 14. Volver a habilitar comprobaciones de FK
ALTER TABLE [TareaTarea]                   WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [AsignacionesProyecto]         WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [Tareas]                       WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [Proyectos]                    WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [Recursos]                     WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [Usuarios]                     WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [AsignacionesRecursoTarea]     WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [UsuarioTarea]                 WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [NotificacionUsuarios]         WITH CHECK CHECK CONSTRAINT ALL;
ALTER TABLE [NotificacionVistas]           WITH CHECK CHECK CONSTRAINT ALL;
GO
