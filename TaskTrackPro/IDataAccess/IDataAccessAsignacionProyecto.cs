using Domain;

namespace IDataAcces;

public interface IDataAccessAsignacionProyecto :IDataAccessGeneric<AsignacionProyecto>
{
    public void Add(AsignacionProyecto asignacionProyecto);

    public void Remove(AsignacionProyecto data);

    public AsignacionProyecto? GetById(int Id);

    public List<AsignacionProyecto> GetAll();

    public List<AsignacionProyecto> AsignacionesDelUsuario(int Id);
    
    public List<AsignacionProyecto> UsuariosDelProyecto(int Id);
    
    public bool UsuarioEsAsignadoAProyecto(int usuarioId, int proyectoid);
    
    public bool UsuarioEsAdminDelProyecto(int usuarioId, int proyectoid);
    
    public AsignacionProyecto GetAdminProyecto(int proyectoId);
    List<Usuario>? GetMiembrosDeProyecto(int id);
}