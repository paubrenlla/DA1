namespace IDataAcces;

public interface IDataAccessGeneric <T>
{
    public void Add(T data);

    public void Remove(T data);

    public T? GetById(int Id);

    public List<T> GetAll();
}