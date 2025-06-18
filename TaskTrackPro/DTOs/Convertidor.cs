using Domain;
namespace DTOs;

public static class Convertidor
{
    public static UsuarioDTO AUsuarioDTO(Usuario usuario)
    {
        return new UsuarioDTO
        {
            Id = usuario.Id,
            Email = usuario.Email,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            FechaNacimiento = usuario.FechaNacimiento,
        };
    }
    
    public static UsuarioConContraseñaDTO AUsuarioConContraseñaDTO(Usuario u)
    {
        return new UsuarioConContraseñaDTO
        {
            Id = u.Id,
            Email = u.Email,
            Nombre = u.Nombre,
            Apellido = u.Apellido,
            FechaNacimiento = u.FechaNacimiento,
            Contraseña = EncriptadorContrasena.DesencriptarPassword(u.Pwd)
        };
    }
    
    public static ProyectoDTO AProyectoDTO(Proyecto p)
    {
        return new ProyectoDTO
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion,
            FechaInicio = p.FechaInicio,
            FinEstimado = p.FinEstimado
        };
    }
    
    public static TareaDTO ATareaDTO(Tarea tarea)
    {
        return new TareaDTO
        {
            Id = tarea.Id,
            Titulo = tarea.Titulo,
            Descripcion = tarea.Descripcion,
            FechaInicio = tarea.FechaInicio,
            Duracion = tarea.Duracion,
            EsCritica = tarea.EsCritica,
            Estado = tarea.EstadoActual.Valor.ToString(),
            Holgura = tarea.Holgura,
            EarlyFinish = tarea.EarlyFinish,
            EarlyStart = tarea.EarlyStart,
        };
    }

    public static RecursoDTO ARecursoDTO(Recurso recurso)
    {
        return new RecursoDTO()
        {
            Id = recurso.Id,
            Nombre = recurso.Nombre,
            Tipo = recurso.Tipo,
            Descripcion = recurso.Descripcion,
            CantidadDelRecurso = recurso.CantidadDelRecurso,
            CantidadEnUso = recurso.CantidadEnUso,
            SePuedeCompartir = recurso.SePuedeCompartir
        };
    }
    
    public static AsignacionRecursoTareaDTO AAsignacionRecursoTareaDTO (AsignacionRecursoTarea asignacionRecursoTarea)
    {
        return new AsignacionRecursoTareaDTO()
        {
            Recurso = ARecursoDTO(asignacionRecursoTarea.Recurso),
            Id = asignacionRecursoTarea.Id,
            Tarea = ATareaDTO(asignacionRecursoTarea.Tarea),
            Cantidad = asignacionRecursoTarea.CantidadNecesaria
        };
    }


    public static NotificacionDTO? ANotificacionDTO(Notificacion? notificacion)
    {
        if (notificacion == null)
        {
            return null;
        }

        return new NotificacionDTO
        {
            Id = notificacion.Id,
            Mensaje = notificacion.Mensaje,
        };
    }
}
