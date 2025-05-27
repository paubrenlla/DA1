using Domain;

namespace Repositorios.DTOs;

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

}
