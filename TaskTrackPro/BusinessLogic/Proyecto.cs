namespace BusinessLogic;

public class Proyecto
{
    private static int _contadorId = 0; // contador compartido entre todas las instancias

    public int Id { get; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }
    public DateTime FechaInicio { get; set; }
    public List<Tarea> TareasAsociadas { get; set; }
    public List<Usuario> Miembros { get; set; }
    
    public List<Recurso> RecursosAsociados { get; set; }
    
    

    public Proyecto(string nombre, string descripcion, DateTime fechaInicio)
    {
        if (fechaInicio.Date < DateTime.Now.Date)
            throw new ArgumentException("La fecha de inicio no puede ser anterior a hoy.");
        
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede ser nulo o vacío.");
        
        if (descripcion.Length > 400)
            throw new ArgumentException("La descripción no puede superar los 400 caracteres.");
        Id = _contadorId++;
        Nombre = nombre;
        Descripcion = descripcion;
        FechaInicio = fechaInicio;
        TareasAsociadas = new List<Tarea>();
        Miembros = new List<Usuario>();
        RecursosAsociados = new List<Recurso>();
    }

    public void agregarTarea(Tarea tarea)
    {
        if (TareasAsociadas.Contains(tarea))
        {
            throw new ArgumentException("La tarea ya existe en el proyecto.");
        }
        TareasAsociadas.Add(tarea);
    }
    
    public void eliminarTarea(Tarea tarea)
    {
        if (!TareasAsociadas.Contains(tarea))
            throw new ArgumentException("No existe la tarea en este proyecto");

        TareasAsociadas.Remove(tarea);
    }

    public void agregarMiembro(Usuario user)
    {
        if (Miembros.Contains(user))
        {
            throw new ArgumentException("Este usuario ya es miembro del proyecto.");
        }
        Miembros.Add(user);
    }
    
    public void eliminarMiembro(Usuario user)
    {
        if (!Miembros.Contains(user))
            throw new ArgumentException("Este usuario no es integrante del proyecto.");

        Miembros.Remove(user);
    }

    public void agregarRecurso(Recurso recurso)
    {
        if (RecursosAsociados.Contains(recurso))
        {
            throw new ArgumentException("El recurso ya es parte del proyecto.");
        }
        RecursosAsociados.Add(recurso);
    }

    public List<Tarea> TareasSinDependencia()
    {
        var tareas = new List<Tarea>(TareasAsociadas.Where(t => t.TareasDependencia.Count == 0));
        return tareas;
    }
}