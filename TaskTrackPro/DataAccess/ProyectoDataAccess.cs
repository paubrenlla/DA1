using BusinessLogic;
using IDataAcces;

namespace Repositorios;

public class ProyectoDataAccess :IDataAccessProyecto
{
    private List<Proyecto> _listaProyectos;
    
    public ProyectoDataAccess()
    {
        _listaProyectos = new List<Proyecto>();
    }
    public void Add(Proyecto proyecto)
    {
        if (_listaProyectos.Contains(proyecto))
            throw new ArgumentException("El proyecto ya existe");
        _listaProyectos.Add(proyecto);
    }
    
    public void Remove(Proyecto proyecto)
    {
       _listaProyectos.Remove(proyecto);
    }
    
    public Proyecto GetById(int id)
    {
        Proyecto proyecto = _listaProyectos.FirstOrDefault(p => p.Id == id);
        if (proyecto == null)
            throw new ArgumentException("No existe el proyecto");
        return proyecto;

    }

    public List<Proyecto> GetAll()
    {
        return _listaProyectos;
    }
    
    public bool esAdminDeAlgunProyecto(Usuario usuario)
    {
        return _listaProyectos.Any(p => p.Admin.Equals(usuario));
    }
    
    public void EliminarAsignacionesDeProyectos(Usuario usuario)
    {
        List<Proyecto> proyectosDelUsuario = ProyectosDelUsuario(usuario);
        foreach (Proyecto proyecto in proyectosDelUsuario)
        {
            proyecto.Miembros.Remove(usuario);
            foreach (Tarea tarea in proyecto.TareasAsociadas)
            {
                if (tarea.UsuariosAsignados.Contains(usuario)) //TODO Ley Demeter, ver si por ejemplo hay Tarea Repo
                {
                    tarea.UsuariosAsignados.Remove(usuario);
                }
            }
        }
    }
    
    public List<Proyecto> ProyectosDelUsuario(Usuario usuario)
    {
        return _listaProyectos
            .Where(p => p.Miembros.Any(m => m.Id == usuario.Id) || p.Admin.Id == usuario.Id)
            .ToList();
    }
}