using System.Data;

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
    public DateTime? FinEstimado { get; set; }
    
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
        Nombre = nombre;
        Descripcion = descripcion;
        FechaInicio = fechaInicio;
        TareasAsociadas = new List<Tarea>();
        Miembros = new List<Usuario>();
        Id = _contadorId++;
    }

    public void agregarTarea(Tarea tarea)
    {
        if (TareasAsociadas.Contains(tarea))
        {
            throw new ArgumentException("La tarea ya existe en el proyecto.");
        }

        foreach (Tarea tareaNueva in TareasAsociadas)
        {
            if (tareaNueva.Titulo == tarea.Titulo)
            {
                throw new ArgumentException("Ya existe una tarea con ese nombre en el proyecto.");
            }
        }
        TareasAsociadas.Add(tarea);
        tarea.Proyecto = this;
        CalcularRutaCritica();
    }
    
    public void eliminarTarea(Tarea tarea)
    {
        if (!TareasAsociadas.Contains(tarea))
            throw new ArgumentException("No existe la tarea en este proyecto");

        TareasAsociadas.Remove(tarea);
        foreach (Tarea tareaDependencia in tarea.TareasDependencia)
        {
            tareaDependencia.TareasSucesoras.Remove(tarea);
            tareaDependencia.ActualizarEstado();
        }
        Notificacion notificacion = new Notificacion("Se eliminado la tarea " + tarea.Titulo + " del proyecto " + Nombre + ".");
        notificacion.AgregarUsuarios(tarea.UsuariosAsignados);
        notificacion.AgregarUsuario(Admin);
        CalcularRutaCritica();
    }

    public void agregarMiembro(Usuario user)
    {
        if (Miembros.Contains(user))
            throw new ArgumentException("Este usuario ya es miembro del proyecto.");
        Miembros.Add(user);
        Notificacion notificacion = new Notificacion("Ha sido agregado al proyecto: " + Nombre + ".");
        notificacion.AgregarUsuario(user);
    }
    
    public void eliminarMiembro(Usuario user)
    {
        if (!Miembros.Contains(user))
            throw new ArgumentException("Este usuario no es integrante del proyecto.");
        
        Miembros.Remove(user);
        
        var tareasConUsuario = TareasAsociadas
            .Where(t => t.UsuariosAsignados.Contains(user));

        foreach (var tarea in tareasConUsuario)
        {
            tarea.UsuariosAsignados.Remove(user);
        }
    }
    
    public void eliminarMiembroTarea(Usuario user, Tarea tarea)
    {
        if (!BuscarTareaPorId(tarea.Id).UsuariosAsignados.Contains(user))
            throw new ArgumentException("Este usuario no es integrante de la tarea.");

        BuscarTareaPorId(tarea.Id).UsuariosAsignados.Remove(user);
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
                tarea.EarlyStart = tarea.FechaInicio;
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
            tarea.EsCritica = tarea.Holgura == TimeSpan.Zero;
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
        Notificacion notificacion = new Notificacion("Ha sido agregado a la tarea " + tarea.Titulo + ".");
        notificacion.AgregarUsuario(usuario);
    }
    
    public void AsignarAdmin(Usuario usuario)
    {
        Admin = usuario;
        Notificacion notificacion = new Notificacion("Eres administrador del proyecto " + Nombre + ".");
        notificacion.AgregarUsuario(usuario);
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
    
    public void CalcularFinEstimado()
    {
        FinEstimado= TareasAsociadas.Max(t=>t.EarlyFinish);
    }

    public List<Tarea> TareasNoCriticas()
    {
        return TareasAsociadas.Where(t => t.Holgura != TimeSpan.Zero).ToList();
    }

    public DateTime InicioVerdadero()
    {
        return TareasAsociadas.Min(t=>t.EarlyStart);
    }
    
    public int CalcularDiasTotales()
    {
        if (TareasAsociadas.Count == 0 || FinEstimado == null)
            return 0;

        return (FinEstimado.Value - InicioVerdadero()).Days + 1;
    }

    public List<Tarea> TareasAsociadasPorInicio()
    {
        return TareasAsociadas
            .OrderBy(t => t.EarlyStart)
            .ToList();
    }
}