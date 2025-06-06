using Domain;

namespace IDataAcces;

public interface IDataAccessTarea : IDataAccessGeneric<Tarea>
{
    List<Tarea> GetTareasDeUsuarioEnProyecto(int miembroId,  int proyectoId);
}