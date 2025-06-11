using Domain;
using System.Collections.Generic;

namespace IDataAcces
{
    public interface IDataAccessRecurso : IDataAccessGeneric<Recurso>
    {
       public void Update(Recurso recurso);
    }
}