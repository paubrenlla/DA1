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

-- Inserciones de datos
INSERT INTO TaskTrackProDB.dbo.AsignacionesProyecto (ProyectoId,UsuarioId,Rol) VALUES
	 (1,1,0),
	 (1,1,2),
	 (2,2,0),
	 (2,4,2),
	 (3,3,0),
	 (3,2,2),
	 (4,3,0),
	 (4,4,2),
	 (5,3,0),
	 (5,5,2);
INSERT INTO TaskTrackProDB.dbo.AsignacionesProyecto (ProyectoId,UsuarioId,Rol) VALUES
	 (6,5,0),
	 (6,6,2),
	 (7,5,0),
	 (7,5,2),
	 (8,5,0),
	 (8,7,2),
	 (9,5,0),
	 (9,7,2),
	 (10,5,0),
	 (10,7,2);
INSERT INTO TaskTrackProDB.dbo.AsignacionesProyecto (ProyectoId,UsuarioId,Rol) VALUES
	 (1,2,1),
	 (1,3,1),
	 (2,3,1),
	 (4,5,1),
	 (4,6,1),
	 (4,7,1),
	 (5,2,1),
	 (5,4,1),
	 (5,6,1),
	 (5,7,1);
INSERT INTO TaskTrackProDB.dbo.AsignacionesProyecto (ProyectoId,UsuarioId,Rol) VALUES
	 (6,9,1),
	 (7,2,1),
	 (7,4,1),
	 (8,3,1),
	 (8,4,1),
	 (8,6,1),
	 (9,2,1),
	 (9,3,1),
	 (10,8,1);
INSERT INTO TaskTrackProDB.dbo.AsignacionesRecursoTarea (RecursoId,TareaId,CantidadNecesaria) VALUES
	 (1,1,1),
	 (1,2,1),
	 (2,1,1),
	 (2,3,2),
	 (4,7,1),
	 (4,8,1),
	 (5,11,2),
	 (5,15,1),
	 (7,17,7),
	 (8,17,9);
INSERT INTO TaskTrackProDB.dbo.AsignacionesRecursoTarea (RecursoId,TareaId,CantidadNecesaria) VALUES
	 (7,15,4),
	 (8,15,6),
	 (7,10,5),
	 (8,10,3),
	 (6,7,4),
	 (6,8,2),
	 (6,10,3),
	 (6,6,2);
INSERT INTO TaskTrackProDB.dbo.NotificacionUsuarios (NotificacionesRecibidasId,UsuariosNotificadosId) VALUES
	 (1,1),
	 (2,1),
	 (4,1),
	 (3,2),
	 (5,2),
	 (6,2),
	 (7,2),
	 (8,2),
	 (9,2),
	 (10,2);
INSERT INTO TaskTrackProDB.dbo.NotificacionUsuarios (NotificacionesRecibidasId,UsuariosNotificadosId) VALUES
	 (11,3),
	 (12,3),
	 (13,3),
	 (14,3),
	 (15,5),
	 (16,5),
	 (17,5),
	 (18,5),
	 (19,5),
	 (20,5);
INSERT INTO TaskTrackProDB.dbo.NotificacionUsuarios (NotificacionesRecibidasId,UsuariosNotificadosId) VALUES
	 (21,5),
	 (22,5),
	 (23,5),
	 (24,5),
	 (25,5),
	 (26,5),
	 (27,5),
	 (28,5),
	 (29,5),
	 (30,5);
INSERT INTO TaskTrackProDB.dbo.NotificacionUsuarios (NotificacionesRecibidasId,UsuariosNotificadosId) VALUES
	 (31,5),
	 (32,5);
INSERT INTO TaskTrackProDB.dbo.Notificaciones (Mensaje) VALUES
	 (N'Se ha agregado la tarea ''Tarea 1'' al proyecto ''Proyecto 1''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha agregado la tarea ''Tarea 2'' al proyecto ''Proyecto 1''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha agregado la tarea ''Tarea 3'' al proyecto ''Proyecto 2''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha modificado una dependencia en la tarea ''Tarea 1'' en el proyecto ''Proyecto 1''.
Es muy probable que la fecha de fin del proyecto haya cambiado!!!'),
	 (N'Se ha agregado la tarea ''Tarea 4'' al proyecto ''Proyecto 2''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha agregado la tarea ''Tarea 5'' al proyecto ''Proyecto 2''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha modificado una dependencia en la tarea ''Tarea 4'' en el proyecto ''Proyecto 2''.
Es muy probable que la fecha de fin del proyecto haya cambiado!!!'),
	 (N'Se ha modificado una dependencia en la tarea ''Tarea 4'' en el proyecto ''Proyecto 2''.
Es muy probable que la fecha de fin del proyecto haya cambiado!!!'),
	 (N'Se ha modificado una dependencia en la tarea ''Tarea 5'' en el proyecto ''Proyecto 2''.
Es muy probable que la fecha de fin del proyecto haya cambiado!!!'),
	 (N'Se ha modificado una dependencia en la tarea ''Tarea 4'' en el proyecto ''Proyecto 2''.
Es muy probable que la fecha de fin del proyecto haya cambiado!!!');
INSERT INTO TaskTrackProDB.dbo.Notificaciones (Mensaje) VALUES
	 (N'Se ha agregado la tarea ''Tarea 6'' al proyecto ''Proyecto 3''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha agregado la tarea ''Tarea 7'' al proyecto ''Proyecto 4''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha agregado la tarea ''Tarea 8'' al proyecto ''Proyecto 4''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha agregado la tarea ''Tarea 9'' al proyecto ''Proyecto 5''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha agregado la tarea ''Tarea 10'' al proyecto ''Proyecto 6''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha agregado la tarea ''Tarea 11'' al proyecto ''Proyecto 7''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha agregado la tarea ''Tarea 12'' al proyecto ''Proyecto 7''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha modificado una dependencia en la tarea ''Tarea 12'' en el proyecto ''Proyecto 7''.
Es muy probable que la fecha de fin del proyecto haya cambiado!!!'),
	 (N'Se ha agregado la tarea ''Tarea 13'' al proyecto ''Proyecto 7''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha modificado una dependencia en la tarea ''Tarea 13'' en el proyecto ''Proyecto 7''.
Es muy probable que la fecha de fin del proyecto haya cambiado!!!');
INSERT INTO TaskTrackProDB.dbo.Notificaciones (Mensaje) VALUES
	 (N'Se ha agregado la tarea ''Tarea 14'' al proyecto ''Proyecto 7''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha modificado una dependencia en la tarea ''Tarea 14'' en el proyecto ''Proyecto 7''.
Es muy probable que la fecha de fin del proyecto haya cambiado!!!'),
	 (N'Se ha agregado la tarea ''Tarea 15'' al proyecto ''Proyecto 8''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha agregado la tarea ''Tarea 16'' al proyecto ''Proyecto 8''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha modificado una dependencia en la tarea ''Tarea 16'' en el proyecto ''Proyecto 8''.
Es muy probable que la fecha de fin del proyecto haya cambiado!!!'),
	 (N'Se ha agregado la tarea ''Tarea 17'' al proyecto ''Proyecto 9''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha agregado la tarea ''Tarea 18'' al proyecto ''Proyecto 9''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha agregado la tarea ''Tarea 19'' al proyecto ''Proyecto 9''.
Esto puede cambiar la fecha de fin del proyecto!!!'),
	 (N'Se ha modificado una dependencia en la tarea ''Tarea 19'' en el proyecto ''Proyecto 9''.
Es muy probable que la fecha de fin del proyecto haya cambiado!!!'),
	 (N'Se ha agregado la tarea ''Tarea 20'' al proyecto ''Proyecto 9''.
Esto puede cambiar la fecha de fin del proyecto!!!');
INSERT INTO TaskTrackProDB.dbo.Notificaciones (Mensaje) VALUES
	 (N'Se ha modificado una dependencia en la tarea ''Tarea 20'' en el proyecto ''Proyecto 9''.
Es muy probable que la fecha de fin del proyecto haya cambiado!!!'),
	 (N'Se ha modificado una dependencia en la tarea ''Tarea 20'' en el proyecto ''Proyecto 9''.
Es muy probable que la fecha de fin del proyecto haya cambiado!!!');
INSERT INTO TaskTrackProDB.dbo.Proyectos (FinEstimado,Nombre,Descripcion,FechaInicio) VALUES
	 ('2025-06-27 11:00:00.0000000',N'Proyecto 1',N'Proyecto 1','2025-06-26 00:00:00.0000000'),
	 ('2025-06-28 16:00:00.0000000',N'Proyecto 2',N'Proyecto 2','2025-06-26 00:00:00.0000000'),
	 ('2025-06-28 15:00:00.0000000',N'Proyecto 3',N'Proyecto 3','2025-06-25 00:00:00.0000000'),
	 ('2025-06-28 03:00:00.0000000',N'Proyecto 4',N'Proyecto 4','2025-06-23 00:00:00.0000000'),
	 ('2025-06-27 09:00:00.0000000',N'Proyecto 5',N'Proyecto 5','2025-06-27 00:00:00.0000000'),
	 ('2025-06-27 07:00:00.0000000',N'Proyecto 6',N'Proyecto 6','2025-06-27 00:00:00.0000000'),
	 ('2025-06-30 07:00:00.0000000',N'Proyecto 7',N'Proyecto 7','2025-06-26 00:00:00.0000000'),
	 ('2025-06-27 22:00:00.0000000',N'Proyecto 8',N'Proyecto 8','2025-06-24 00:00:00.0000000'),
	 ('2025-06-30 03:00:00.0000000',N'Proyecto 9',N'Proyecto 9','2025-06-25 00:00:00.0000000'),
	 (NULL,N'Proyecto 10',N'Proyecto 10','2025-06-27 00:00:00.0000000');
INSERT INTO TaskTrackProDB.dbo.Recursos (Nombre,Tipo,Descripcion,SePuedeCompartir,CantidadDelRecurso,CantidadEnUso) VALUES
	 (N'Recurso 1 - 1T',N'Recurso',N'Recurso',1,1,0),
	 (N'Recurso 2 - 2T',N'Recurso',N'Recurso',1,2,0),
	 (N'Recurso 3 - 5T',N'Recurso',N'Recurso',1,5,0),
	 (N'Recurso 4 - 1F',N'Recurso',N'Recurso',0,1,0),
	 (N'Recurso 5 - 2F',N'Recurso',N'Recurso',0,2,0),
	 (N'Recurso 6 - 5F',N'Recurso',N'Recurso',0,5,0),
	 (N'Recurso 7 - 10T',N'Recurso',N'Recurso',1,10,0),
	 (N'Recurso 8 - 10F',N'Recurso',N'Recurso',0,10,0),
	 (N'Recurso 9',N'Recurso 9',N'Recurso 9',1,3,0);
INSERT INTO TaskTrackProDB.dbo.TareaTarea (TareasDependenciaId,TareasSucesorasId) VALUES
	 (2,1),
	 (3,4),
	 (4,5),
	 (11,12),
	 (12,13),
	 (12,14),
	 (15,16),
	 (17,19),
	 (18,20),
	 (19,20);
INSERT INTO TaskTrackProDB.dbo.Tareas (EarlyStart,LateStart,EarlyFinish,LateFinish,Titulo,Descripcion,FechaInicio,EsCritica,EstadoValor,EstadoFecha,ProyectoId,Holgura,Duracion,RecursosForzados) VALUES
	 ('2025-06-26 06:00:00.0000000','2025-06-26 06:00:00.0000000','2025-06-27 11:00:00.0000000','2025-06-27 11:00:00.0000000',N'Tarea 1',N'Tarea 1','2025-06-27 00:00:00.0000000',1,1,'2025-06-17 18:56:38.0764904',1,0,1044000000000,0),
	 ('2025-06-26 00:00:00.0000000','2025-06-26 00:00:00.0000000','2025-06-26 06:00:00.0000000','2025-06-26 06:00:00.0000000',N'Tarea 2',N'Tarea 2','2025-06-26 00:00:00.0000000',1,0,NULL,1,0,216000000000,0),
	 ('2025-06-26 00:00:00.0000000','2025-06-26 00:00:00.0000000','2025-06-26 03:00:00.0000000','2025-06-26 03:00:00.0000000',N'Tarea 3',N'Tarea 3','2025-06-26 00:00:00.0000000',1,0,NULL,2,0,108000000000,0),
	 ('2025-06-26 03:00:00.0000000','2025-06-26 03:00:00.0000000','2025-06-26 09:00:00.0000000','2025-06-26 09:00:00.0000000',N'Tarea 4',N'Tarea 4','2025-06-27 00:00:00.0000000',1,1,'2025-06-17 18:58:08.7959319',2,0,216000000000,0),
	 ('2025-06-26 09:00:00.0000000','2025-06-26 09:00:00.0000000','2025-06-28 16:00:00.0000000','2025-06-28 16:00:00.0000000',N'Tarea 5',N'Tarea 5','2025-06-27 00:00:00.0000000',1,1,'2025-06-17 18:58:04.2573143',2,0,1980000000000,0),
	 ('2025-06-25 00:00:00.0000000','2025-06-25 00:00:00.0000000','2025-06-28 15:00:00.0000000','2025-06-28 15:00:00.0000000',N'Tarea 6',N'Tarea 6','2025-06-25 00:00:00.0000000',1,0,NULL,3,0,3132000000000,0),
	 ('2025-06-23 00:00:00.0000000','2025-06-23 00:00:00.0000000','2025-06-28 03:00:00.0000000','2025-06-28 03:00:00.0000000',N'Tarea 7',N'Tarea 7','2025-06-23 00:00:00.0000000',1,0,NULL,4,0,4428000000000,0),
	 ('2025-06-25 00:00:00.0000000','2025-06-25 18:00:00.0000000','2025-06-27 09:00:00.0000000','2025-06-28 03:00:00.0000000',N'Tarea 8',N'Tarea 8','2025-06-25 00:00:00.0000000',0,0,NULL,4,648000000000,2052000000000,0),
	 ('2025-06-27 00:00:00.0000000','2025-06-27 00:00:00.0000000','2025-06-27 09:00:00.0000000','2025-06-27 09:00:00.0000000',N'Tarea 9',N'Tarea 9','2025-06-27 00:00:00.0000000',1,0,NULL,5,0,324000000000,0),
	 ('2025-06-27 00:00:00.0000000','2025-06-27 00:00:00.0000000','2025-06-27 07:00:00.0000000','2025-06-27 07:00:00.0000000',N'Tarea 10',N'Tarea 10','2025-06-27 00:00:00.0000000',1,0,NULL,6,0,252000000000,0);
INSERT INTO TaskTrackProDB.dbo.Tareas (EarlyStart,LateStart,EarlyFinish,LateFinish,Titulo,Descripcion,FechaInicio,EsCritica,EstadoValor,EstadoFecha,ProyectoId,Holgura,Duracion,RecursosForzados) VALUES
	 ('2025-06-26 00:00:00.0000000','2025-06-26 00:00:00.0000000','2025-06-28 00:00:00.0000000','2025-06-28 00:00:00.0000000',N'Tarea 11',N'Tarea 11','2025-06-26 00:00:00.0000000',1,0,NULL,7,0,1728000000000,0),
	 ('2025-06-28 00:00:00.0000000','2025-06-28 00:00:00.0000000','2025-06-29 06:00:00.0000000','2025-06-29 06:00:00.0000000',N'Tarea 12',N'Tarea 12','2025-06-26 00:00:00.0000000',1,1,'2025-06-17 19:00:55.2859753',7,0,1080000000000,0),
	 ('2025-06-29 06:00:00.0000000','2025-06-29 22:00:00.0000000','2025-06-29 15:00:00.0000000','2025-06-30 07:00:00.0000000',N'Tarea 13',N'Tarea 13','2025-06-28 00:00:00.0000000',0,1,'2025-06-17 19:01:25.3060967',7,576000000000,324000000000,0),
	 ('2025-06-29 06:00:00.0000000','2025-06-29 06:00:00.0000000','2025-06-30 07:00:00.0000000','2025-06-30 07:00:00.0000000',N'Tarea 14',N'Tarea 14','2025-06-28 00:00:00.0000000',1,1,'2025-06-17 19:01:53.4850979',7,0,900000000000,0),
	 ('2025-06-26 00:00:00.0000000','2025-06-26 00:00:00.0000000','2025-06-26 05:00:00.0000000','2025-06-26 05:00:00.0000000',N'Tarea 15',N'Tarea 15','2025-06-26 00:00:00.0000000',1,0,NULL,8,0,180000000000,0),
	 ('2025-06-26 05:00:00.0000000','2025-06-26 05:00:00.0000000','2025-06-27 22:00:00.0000000','2025-06-27 22:00:00.0000000',N'Tarea 16',N'Tarea 16','2025-06-27 00:00:00.0000000',1,1,'2025-06-17 19:03:01.2366906',8,0,1476000000000,0),
	 ('2025-06-25 00:00:00.0000000','2025-06-25 06:00:00.0000000','2025-06-25 18:00:00.0000000','2025-06-26 00:00:00.0000000',N'Tarea 17',N'Tarea 17','2025-06-25 00:00:00.0000000',0,0,NULL,9,216000000000,648000000000,0),
	 ('2025-06-25 00:00:00.0000000','2025-06-25 00:00:00.0000000','2025-06-26 03:00:00.0000000','2025-06-26 03:00:00.0000000',N'Tarea 18',N'Tarea 18','2025-06-25 00:00:00.0000000',1,0,NULL,9,0,972000000000,0),
	 ('2025-06-25 18:00:00.0000000','2025-06-26 00:00:00.0000000','2025-06-25 21:00:00.0000000','2025-06-26 03:00:00.0000000',N'Tarea 19',N'Tarea 19','2025-06-25 00:00:00.0000000',0,1,'2025-06-17 19:04:15.6147038',9,216000000000,108000000000,0),
	 ('2025-06-26 03:00:00.0000000','2025-06-26 03:00:00.0000000','2025-06-30 03:00:00.0000000','2025-06-30 03:00:00.0000000',N'Tarea 20',N'Tarea 20','2025-06-27 00:00:00.0000000',1,1,'2025-06-17 19:04:42.4142566',9,0,3456000000000,0);
INSERT INTO TaskTrackProDB.dbo.UsuarioTarea (TareasAsignadasId,UsuariosAsignadosId) VALUES
	 (1,1),
	 (1,2),
	 (9,2),
	 (11,2),
	 (12,2),
	 (17,2),
	 (18,2),
	 (19,2),
	 (2,3),
	 (4,3);
INSERT INTO TaskTrackProDB.dbo.UsuarioTarea (TareasAsignadasId,UsuariosAsignadosId) VALUES
	 (15,3),
	 (18,3),
	 (19,3),
	 (3,4),
	 (4,4),
	 (12,4),
	 (14,4),
	 (16,4),
	 (7,5),
	 (14,5);
INSERT INTO TaskTrackProDB.dbo.UsuarioTarea (TareasAsignadasId,UsuariosAsignadosId) VALUES
	 (19,5),
	 (7,6),
	 (9,6),
	 (15,6),
	 (8,7),
	 (9,7),
	 (19,7),
	 (10,9);
INSERT INTO TaskTrackProDB.dbo.Usuarios (EsAdminSistema,Email,Nombre,Apellido,Pwd,PwdSerializado,FechaNacimiento) VALUES
	 (1,N'admin@ejemplo.com',N'Ada',N'Lovelace',N'Q29udHJhc2XDsWExIQ==',N'Q29udHJhc2XDsWExIQ==','1985-12-10 00:00:00.0000000'),
	 (0,N'usuario1@mail.com',N'Mateo',N'Muniz',N'Q29udHJhc2VuYTEh',N'Q29udHJhc2VuYTEh','2000-01-01 00:00:00.0000000'),
	 (0,N'usuario2@mail.com',N'Paula',N'Brenlla',N'Q29udHJhc2VuYTEh',N'Q29udHJhc2VuYTEh','2000-01-01 00:00:00.0000000'),
	 (0,N'usuario3@mail.com',N'Fernando',N'Gavello',N'Q29udHJhc2VuYTEh',N'Q29udHJhc2VuYTEh','2000-01-01 00:00:00.0000000'),
	 (0,N'usuario4@mail.com',N'Luis',N'Simon',N'Q29udHJhc2VuYTEh',N'Q29udHJhc2VuYTEh','2000-01-01 00:00:00.0000000'),
	 (0,N'usuario5@mail.com',N'Santiago',N'Perez',N'Q29udHJhc2VuYTEh',N'Q29udHJhc2VuYTEh','2000-01-01 00:00:00.0000000'),
	 (0,N'usuario6@mail.com',N'Juan',N'Diaz',N'Q29udHJhc2VuYTEh',N'Q29udHJhc2VuYTEh','2000-01-01 00:00:00.0000000'),
	 (0,N'usuario7@mail.com',N'Harry',N'Potter',N'Q29udHJhc2VuYTEh',N'Q29udHJhc2VuYTEh','1980-07-31 00:00:00.0000000'),
	 (0,N'usuario8@mail.com',N'George R R',N'Martin',N'Q29udHJhc2VuYTEh',N'Q29udHJhc2VuYTEh','1948-09-20 00:00:00.0000000'),
	 (0,N'usuario9@mail.com',N'J R R',N'Tolkien',N'Q29udHJhc2XDsWExIQ==',N'Q29udHJhc2XDsWExIQ==','1892-01-03 00:00:00.0000000');
