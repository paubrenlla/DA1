using DTOs;

namespace Services;

public interface IRecursoService
{
    RecursoDTO GetById(int idRecurso);
        
    List<RecursoDTO> GetAll();
        
    RecursoDTO Add(RecursoDTO dto);
        
    void Delete(int idRecurso);
        
    void ModificarRecurso(RecursoDTO dto);

    bool EstaEnUso(int idRecurso);

    void ConsumirRecurso(int idRecurso, int cantidad);

    void LiberarRecurso(int idRecurso, int cantidad);

    bool EstaDisponible(int idRecurso, int cantidad);

}