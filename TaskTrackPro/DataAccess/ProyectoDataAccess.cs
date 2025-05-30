using Domain;
using IDataAcces;

namespace DataAccess;

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
    
}