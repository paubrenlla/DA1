namespace BusinessLogic;

public class Proyecto
{
    private static int _contadorId = 1;

    public int Id { get; }
    private string _nombre;
    private string _descripcion;
    private DateTime _fechaInicio;
    public List<Tarea> TareasAsociadas { get; set; }
    public List<Usuario> Miembros { get; set; }
    public Usuario Admin { get; set; }
    
    public List<Recurso> RecursosAsociados { get; set; }
    
    public string Nombre
    {
        get => _nombre;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("El nombre no puede estar vacío.");
            _nombre = value;
        }
    }
    
    public string Descripcion
    {
        get => _descripcion;
        set
        {
            if (value.Length > 400)
                throw new ArgumentException("La descripción no puede superar los 400 caracteres.");
            _descripcion = value;
        }
    }
    
    public DateTime FechaInicio
    {
        get => _fechaInicio;
        set
        {
            if (value < DateTime.Now.Date)
                throw new ArgumentException("La fecha de inicio no puede ser anterior a hoy.");
            _fechaInicio = value;
        }
    }
    

    public Proyecto(string nombre, string descripcion, DateTime fechaInicio)
    {
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
    private bool TodasLasDependenciasFueronProcesadas(Tarea tareaQueLeSigue)
    {
        return tareaQueLeSigue.TareasDependencia.All(p => p.EarlyFinish != DateTime.MinValue);
    }

    private bool TodasLasSucesorasFueronProcesadas(Tarea tareaQueLeSigue)
    {
        return tareaQueLeSigue.TareasSucesoras.All(s => s.LateStart != DateTime.MaxValue);

    }
    public void CalcularTiemposTempranos()
    {
        CalcularEarlyTimes();    
    }

    private void CalcularEarlyTimes()
    {
        foreach (Tarea tarea in TareasAsociadas)
        {
            tarea.EarlyStart = DateTime.MinValue;
            tarea.EarlyFinish = DateTime.MinValue;
        }

        Queue<Tarea> pendientes = new Queue<Tarea>(TareasSinDependencia());

        while (pendientes.Count > 0)
        {
            Tarea tarea = pendientes.Dequeue();

            if (tarea.TareasDependencia.Count == 0)
            {
                tarea.EarlyStart = FechaInicio;
            }
            else
            {
                DateTime maxFinish = DateTime.MinValue;
                foreach (Tarea pre in tarea.TareasDependencia)
                {
                    if (pre.EarlyFinish > maxFinish)
                        maxFinish = pre.EarlyFinish;
                }
                tarea.EarlyStart = maxFinish;
            }

            tarea.EarlyFinish = tarea.EarlyStart + tarea.Duracion;

            foreach (Tarea tareaQueLeSigue in TareasAsociadas)
            {
                if (tareaQueLeSigue.TareasDependencia.Contains(tarea))
                {
                    if (TodasLasDependenciasFueronProcesadas(tareaQueLeSigue))
                    {
                        pendientes.Enqueue(tareaQueLeSigue);
                    }
                }
            }
        }
    }
    public void CalcularTiemposTardios()
    {
        DateTime finProyecto = TareasAsociadas.Max(t => t.EarlyFinish);

        foreach (Tarea tarea in TareasAsociadas)
        {
            tarea.LateStart = DateTime.MaxValue;
            tarea.LateFinish = DateTime.MaxValue;
        }

        Queue<Tarea> pendientes = new Queue<Tarea>(TareasAsociadas.Where(t => t.TareasSucesoras.Count == 0));
    
        foreach (Tarea tarea in pendientes)
        {
            tarea.LateFinish = finProyecto;
            tarea.LateStart = tarea.LateFinish - tarea.Duracion;
        }

        while (pendientes.Count > 0)
        {
            Tarea tarea = pendientes.Dequeue();

            foreach (Tarea predecesora in tarea.TareasDependencia)
            {
                if (predecesora.LateFinish > tarea.LateStart)
                {
                    predecesora.LateFinish = tarea.LateStart;
                    predecesora.LateStart = predecesora.LateFinish - predecesora.Duracion;
                }

                if (TodasLasSucesorasFueronProcesadas(predecesora))
                {
                    pendientes.Enqueue(predecesora);
                }
            }
        }
    }
    public List<Tarea> CalcularRutaCritica()
    {
        CalcularTiemposTempranos();
        CalcularTiemposTardios();

        foreach (Tarea tarea in TareasAsociadas)
        {
            tarea.Holgura = tarea.LateStart - tarea.EarlyStart;
        }

        return TareasAsociadas.Where(t => t.Holgura == TimeSpan.Zero).ToList();
    }


    public void AsignarUsuarioATarea(Usuario usuario, Tarea tarea)
    {
        if (!Miembros.Contains(usuario))
            throw new ArgumentException("El usuario no pertenece al proyecto");
    
        if (!TareasAsociadas.Contains(tarea))
            throw new ArgumentException("La tarea no pertenece al proyecto");
    
        if (tarea.UsuariosAsignados.Contains(usuario))
            throw new ArgumentException("El usuario ya está asignado a esta tarea");
    
        tarea.AgregarUsuario(usuario);
    }
    
    public void AsignarAdmin(Usuario usuario)
    {
        Admin = usuario;
    }
    
    public bool EsAdmin(Usuario usuario)
    {
        return usuario.Equals(Admin);
    }
    
    public void Modificar(string descripcionNueva, DateTime fechaInicioNueva)
    {
       Descripcion = descripcionNueva;
       FechaInicio = fechaInicioNueva;
    }
    
    public Tarea? BuscarTareaPorId(int id)
    {
        return TareasAsociadas.FirstOrDefault(r => r.Id == id);
    }
}