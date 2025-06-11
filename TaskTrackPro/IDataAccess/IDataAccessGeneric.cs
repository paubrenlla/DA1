namespace IDataAcces;

public interface IDataAccessGeneric <T>
{
    public void Add(T asignacionProyecto);

    public void Remove(T data);

    public T? GetById(int Id);
    
   // public void Update(T data); TODO cuando termine con integracion a DataBase

    public List<T> GetAll();
}