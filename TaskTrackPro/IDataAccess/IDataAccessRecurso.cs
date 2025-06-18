using Domain;

namespace IDataAcces
{
    public interface IDataAccessRecurso : IDataAccessGeneric<Recurso>
    {
       public void Update(Recurso recurso);
    }
}