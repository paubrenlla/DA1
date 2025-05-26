using BusinessLogic;

namespace IDataAcces;

public interface IDataAccessProyecto : IDataAccessGeneric<Proyecto>
{
    public List<Proyecto> ProyectosDelUsuario(Usuario usuario);

    public void EliminarAsignacionesDeProyectos(Usuario usuario);
    
    public bool EsAdminDeAlgunProyecto(Usuario usuario);
}