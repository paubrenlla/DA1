using Domain;

namespace IDataAcces;

public interface IDataAccessProyecto : IDataAccessGeneric<Proyecto>
{
    public void Update(Proyecto proyecto);
}