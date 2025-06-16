using Domain;

namespace IDataAcces;

public interface IDataAccessProyecto : IDataAccessGeneric<Proyecto>
{
    string ExportarJSON();
    string ExportarCSV();
}