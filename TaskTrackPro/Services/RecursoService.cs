using Domain;
using Domain.Observers;
using DTOs;
using IDataAcces;

namespace Services;

public class RecursoService : IRecursoService
{
    private readonly IDataAccessRecurso _recursoRepo;
    private readonly IDataAccessAsignacionRecursoTarea _asignacionRepo;
    private readonly List<IRecursoObserver> _observadores;
    
    public RecursoService(
        IDataAccessRecurso recursoRepo,
        IDataAccessAsignacionRecursoTarea asignacionRepo,
        IEnumerable<IRecursoObserver> observadores)
    {
        _recursoRepo = recursoRepo;
        _asignacionRepo = asignacionRepo;
        _observadores = observadores.ToList();
    }
    
    public RecursoDTO GetById(int idRecurso)
    {
        Recurso recurso = _recursoRepo.GetById(idRecurso);
        if (recurso == null) throw new NullReferenceException("Recurso");
        return Convertidor.ARecursoDTO(recurso);
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
        
        _recursoRepo.Update(recurso);
        
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
        _recursoRepo.Update(recurso);
    }

    public void LiberarRecurso(int idRecurso, int cantidad)
    {
        Recurso recurso = _recursoRepo.GetById(idRecurso);
        recurso.LiberarRecurso(cantidad);
        
        _recursoRepo.Update(recurso);
        
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