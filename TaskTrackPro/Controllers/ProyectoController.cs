using BusinessLogic;
using Controllers.DTOs;
using IDataAcces;

namespace Controllers;

public class ProyectoController
{
    private IDataAccessProyecto _repoProyectos;
    private IDataAccessUsuario _repoUsuarios;

    public ProyectoController(IDataAccessProyecto p, IDataAccessUsuario u)
    {
        _repoProyectos = p;
        _repoUsuarios = u;
    }

    public ProyectoDTO BuscarProyectoPorId(int id)
    {
        Proyecto? proyecto = _repoProyectos.GetById(id);
        return Convertidor.AProyectoDTO(proyecto);
    }

    public List<ProyectoDTO> GetAll()
    {
        return _repoProyectos.GetAll()
            .Select(p 
                => Convertidor.AProyectoDTO(p))
            .ToList();
    }

    public void AgregarProyecto(ProyectoDTO dto, DateTime fechaInicio)
    {
        Proyecto nuevo = new Proyecto(dto.Nombre, dto.Descripcion, fechaInicio);
        _repoProyectos.Add(nuevo);
    }

    public void EliminarProyecto(int id)
    {
        Proyecto? p = _repoProyectos.GetById(id);
        //TODO eliminar asignaciones de recursos
        _repoProyectos.Remove(p);
    }

    public void ModificarProyecto(ProyectoDTO dto, DateTime fechaInicio)
    {
        Proyecto? p = _repoProyectos.GetById(dto.Id);
        p.Modificar(dto.Descripcion, fechaInicio);
    }
    
    public bool EsAdminDeAlgunProyecto(UsuarioDTO usuario)
    {
        Usuario u = _repoUsuarios.GetById(usuario.Id);
        return _repoProyectos.EsAdminDeAlgunProyecto(u);
    }
    
    public void EliminarAsignacionesDeProyectos(UsuarioDTO usuario)
    {
        Usuario u = _repoUsuarios.GetById(usuario.Id);
        _repoProyectos.EliminarAsignacionesDeProyectos(u);
    }
    
    public List<ProyectoDTO> ProyectosDelUsuario(UsuarioDTO usuario)
    {
        Usuario u = _repoUsuarios.GetById(usuario.Id);
        var proyectos = _repoProyectos.ProyectosDelUsuario(u);
        return proyectos.Select(p => Convertidor.AProyectoDTO(p)).ToList();
    }

    public bool UsuarioEsAdminDelProyecto(UsuarioDTO usuarioDto, ProyectoDTO proyectoDto)
    {
        Proyecto proyecto = _repoProyectos.GetById(proyectoDto.Id);
        Usuario usuario = _repoUsuarios.GetById(usuarioDto.Id);
        return proyecto.Admin.Equals(usuario);
    }

    

}