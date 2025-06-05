using Domain;
using IDataAcces;

namespace DataAccess;

public class RecursoDataAccess :IDataAccessRecurso
{
    private List<Recurso> _listaRecursos;
    
    public RecursoDataAccess()
    {
        _listaRecursos = new List<Recurso>();

        Recurso recurso1 = new Recurso(
            "Auto Rojo",
            "Vehiculo",
            "Auto rojo electrico",
            false,
            1
        );
        _listaRecursos.Add(recurso1);
        
        Recurso recurso2 = new Recurso(
            "Desarrollador Frontend",
            "Humano",
            "Experto en tecnologías como React, Angular y Vue.js",
            true,
            2
        );
        _listaRecursos.Add(recurso2);

        Recurso recurso3 = new Recurso(
            "Desarrollador Backend",
            "Humano",
            "Especialista en Node.js, Python y bases de datos SQL/NoSQL",
            true,
            3
        );
        _listaRecursos.Add(recurso3);

        Recurso recurso4 = new Recurso(
            "Desarrollador Full Stack",
            "Humano",
            "Con habilidades tanto en frontend como en backend",
            true,
            4
        );
        _listaRecursos.Add(recurso4);

        Recurso recurso5 = new Recurso(
            "Laptop de Desarrollo",
            "Equipo Electrónico",
            "Computadora portátil con alto rendimiento para programación",
            false,
            5
        );
        _listaRecursos.Add(recurso5);

        Recurso recurso6 = new Recurso(
            "Servidor Dedicado",
            "Equipo Electrónico",
            "Máquina potente para alojar aplicaciones y servicios web",
            false,
            6
        );
        _listaRecursos.Add(recurso6);
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