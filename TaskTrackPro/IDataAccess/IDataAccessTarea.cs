using Domain;

namespace IDataAcces;

public interface IDataAccessTarea : IDataAccessGeneric<Tarea>
{
    public void Update(Tarea tarea);
}