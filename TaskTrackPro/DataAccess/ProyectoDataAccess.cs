using Domain;
using IDataAcces;

namespace DataAccess;

public class ProyectoDataAccess :IDataAccessProyecto
{
    private List<Proyecto> _listaProyectos;
    
    public ProyectoDataAccess()
    {
        _listaProyectos = new List<Proyecto>();

        Proyecto proyecto1 = new Proyecto(
            "Proyecto 1",
            "Descripcion1",
            DateTime.Now.Add(new TimeSpan(3, 0, 0, 0))
        );
        _listaProyectos.Add(proyecto1);
        
        Proyecto proyecto2 = new Proyecto(
            "Proyecto 2",
            "Descripcion2",
            DateTime.Now.Add(new TimeSpan(40, 0, 0, 0))
        );
        _listaProyectos.Add(proyecto2);
        
        Proyecto proyecto3 = new Proyecto(
            "Proyecto 3",
            "Descripcion3",
            DateTime.Now
        );
        _listaProyectos.Add(proyecto3);
        
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