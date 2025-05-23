using BusinessLogic;

namespace IDataAcces;

public interface IDataAccessProyecto : IDataAccessGeneric<Proyecto>
{
    public List<Proyecto> ProyectosDeUsuario(Usuario usuario);

    public void EliminarAsignacionesDeProyectos(Usuario usuario);

    public List<Proyecto> ProyectosDelUsuario(Usuario usuario);

    public bool esAdminDeAlgunProyecto(Usuario usuario);
}