namespace Domain.Observers;

public interface ITareaObserver
{
    void TareaEliminada(Proyecto proyecto, Tarea tareaEliminada);
    
    void TareaAgregada(Proyecto proyecto, Tarea tareaAgregada);
}