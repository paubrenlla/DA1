using DTOs;
using Services;

namespace Controllers;

public class RecursoController
{
    private readonly IRecursoService _service;

    public RecursoController(IRecursoService service)
    {
        _service = service;
    }
    public RecursoDTO GetById(int idRecurso)
    {
        return _service.GetById(idRecurso);
    }

    public List<RecursoDTO> GetAll()
    {
        return _service.GetAll();
    }

    public RecursoDTO Add(RecursoDTO dto)
    {
        return _service.Add(dto);
    }

    public void Delete(int idRecurso)
    {
        _service.Delete(idRecurso);
    }

    public void ModificarRecurso(RecursoDTO dto)
    {
        _service.ModificarRecurso(dto);
    }

    public bool EstaEnUso(int idRecurso)
    {
        return _service.EstaEnUso(idRecurso);
    }

    public bool EstaDisponible(int idRecurso, int cantidad)
    {
        return _service.EstaDisponible(idRecurso, cantidad);
    }
}