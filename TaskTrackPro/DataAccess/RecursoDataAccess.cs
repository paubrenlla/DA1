using Domain;
using IDataAcces;

namespace DataAccess;

public class RecursoDataAccess :IDataAccessRecurso
{
    private List<Recurso> _listaRecursos;
    
    public RecursoDataAccess()
    {
        _listaRecursos = new List<Recurso>();
    }
    public void Add(Recurso recurso)
    {
        if (_listaRecursos.Contains(recurso))
            throw new ArgumentException("El recurso ya existe en el sistema.");
        _listaRecursos.Add(recurso);
    }

    public Recurso? GetById(int Id)
    {
       return _listaRecursos.FirstOrDefault(r => r.Id == Id);
    }

    public List<Recurso> GetAll()
    {
        return _listaRecursos;
    }

    public void Remove(Recurso recurso)
    {
        if (recurso.EstaEnUso())
            throw new ArgumentException("No se puede eliminar un recurso que está en uso.");
        _listaRecursos.Remove(recurso);
    }
}