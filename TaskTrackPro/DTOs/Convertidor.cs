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
            Contraseña = usuario.Pwd
        };
    }
    
    public static ProyectoDTO AProyectoDTO(Proyecto p)
    {
        return new ProyectoDTO
        {
            Id = p.Id,
            Nombre = p.Nombre,
            Descripcion = p.Descripcion
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
            Estado = tarea.EstadoActual.Valor.ToString()
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
