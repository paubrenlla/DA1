USE TaskTrackProDB;

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

-- 3. Reseed de identidades ANTES de los INSERT para que arranquen en 1
DBCC CHECKIDENT ('Usuarios',                 RESEED, 0);
DBCC CHECKIDENT ('Recursos',                 RESEED, 0);
DBCC CHECKIDENT ('Proyectos',                RESEED, 0);
DBCC CHECKIDENT ('Tareas',                   RESEED, 0);
DBCC CHECKIDENT ('AsignacionesProyecto',     RESEED, 0);
DBCC CHECKIDENT ('AsignacionesRecursoTarea', RESEED, 0);
DBCC CHECKIDENT ('Notificaciones',           RESEED, 0);
-- Las tablas de unión no tienen identidad

-- 4. Insertar SOLO Usuario Admin
INSERT INTO [Usuarios] (EsAdminSistema, Email, Nombre, Apellido, Pwd, PwdSerializado, FechaNacimiento)
VALUES 
  (1, 'admin@ejemplo.com', 'Ada', 'Lovelace', 'Q29udHJhc2XDsWExIQ==','Q29udHJhc2XDsWExIQ==','1985-12-10T00:00:00');