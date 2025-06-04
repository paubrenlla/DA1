using Domain;
using Domain.Observers;
using DTOs;
using IDataAcces;

namespace Services;

public class RecursoService : IRecursoService
{
    private readonly IDataAccessRecurso _recursoRepo;
    private readonly IDataAccessAsignacionRecursoTarea _asignacionRepo;
    private readonly List<IRecursoObserver> _observadores = new List<IRecursoObserver>();
    
    public RecursoService(
        IDataAccessRecurso recursoRepo)
    {
        _recursoRepo = recursoRepo;
    }
    
    public RecursoDTO GetById(int idRecurso)
    {
        return Convertidor.ARecursoDTO(_recursoRepo.GetById(idRecurso));
    }

    public List<RecursoDTO> GetAll()
    {
        return _recursoRepo.GetAll().Select(Convertidor.ARecursoDTO).ToList();
    }

    public RecursoDTO Add(RecursoDTO dto)
    {
        Recurso nuevoRecurso = new Recurso(dto.Nombre, dto.Tipo, dto.Descripcion, dto.SePuedeCompartir, dto.CantidadDelRecurso);
        _recursoRepo.Add(nuevoRecurso);
        
        return Convertidor.ARecursoDTO(nuevoRecurso);
    }

    public void Delete(int idRecurso)
    {
        Recurso recurso = _recursoRepo.GetById(idRecurso);

        if (_asignacionRepo.GetByRecurso(recurso.Id).Count > 0) 
            throw new ArgumentOutOfRangeException(nameof(recurso.Nombre), "El recurso esta asignado a alguna tarea.");
        
        _recursoRepo.Remove(recurso);
    }

    public void ModificarRecurso(RecursoDTO dto)
    {
        Recurso recurso = _recursoRepo.GetById(dto.Id);
        recurso.Modificar(dto.Nombre, dto.Tipo, dto.Descripcion, dto.CantidadDelRecurso, dto.SePuedeCompartir);

        NotificarObservadores(recurso);
    }

    public bool EstaEnUso(int idRecurso)
    {
        Recurso recurso = _recursoRepo.GetById(idRecurso);
        return recurso.EstaEnUso();
    }

    public void ConsumirRecurso(int idRecurso, int cantidad)
    {
        Recurso recurso = _recursoRepo.GetById(idRecurso);
        recurso.ConsumirRecurso(cantidad);
        NotificarObservadores(recurso);
    }

    public void LiberarRecurso(int idRecurso, int cantidad)
    {
        Recurso recurso = _recursoRepo.GetById(idRecurso);
        recurso.LiberarRecurso(cantidad);
        
        NotificarObservadores(recurso);
    }

    public bool EstaDisponible(int idRecurso, int cantidad)
    {
        Recurso recurso = _recursoRepo.GetById(idRecurso);
        return recurso.EstaDisponible(cantidad);
    }
    
    public void AgregarObservador(IRecursoObserver observer)
    {
        _observadores.Add(observer);
    }

    private void NotificarObservadores(Recurso recurso)
    {
        foreach (IRecursoObserver observer in _observadores)
        {
            observer.ActualizarTareasDeRecurso(recurso);
        }
    }
}