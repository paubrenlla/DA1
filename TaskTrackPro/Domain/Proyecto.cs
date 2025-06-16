namespace Domain;

public class Proyecto
{
    private static int _contadorId = 1;

    public int Id { get; set; }
    private string _nombre;
    private string _descripcion;
    private DateTime _fechaInicio;
    public List<Tarea> TareasAsociadas { get; set; }

    public List<AsignacionProyecto> AsignacionesDelProyecto { get; set;}
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
        AsignacionesDelProyecto = new List<AsignacionProyecto>();
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
    }

    public void agregarMiembro(AsignacionProyecto asignacion)
    {
        if (AsignacionesDelProyecto.Contains(asignacion))
            throw new ArgumentException("Este usuario ya fué asignado al proyecto.");
        AsignacionesDelProyecto.Add(asignacion);
       // Notificacion notificacion = new Notificacion("Ha sido agregado al proyecto: " + Nombre + ".");
       // notificacion.AgregarUsuario(user);
    }
    
    public void eliminarMiembro(AsignacionProyecto asignacion)
    {
        if (!AsignacionesDelProyecto.Contains(asignacion))
            throw new ArgumentException("Este usuario no es integrante del proyecto.");
        
        AsignacionesDelProyecto.Remove(asignacion);
    }
    
    public void eliminarMiembroTarea(Usuario user, Tarea tarea)
    {
        if (!BuscarTareaPorId(tarea.Id).UsuariosAsignados.Contains(user))
            throw new ArgumentException("Este usuario no es integrante de la tarea.");

        BuscarTareaPorId(tarea.Id).UsuariosAsignados.Remove(user);
    }

    public List<Tarea> TareasSinDependencia()
    {
        List<Tarea> tareas = new List<Tarea>(TareasAsociadas.Where(t => t.TareasDependencia.Count == 0));
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

   public void CalcularTiemposTempranos(IEnumerable<AsignacionRecursoTarea> asignaciones)
        {
            foreach (Tarea tarea in TareasAsociadas)
            {
                if (!tarea.EstaCompleta())
                {
                    tarea.EarlyStart  = DateTime.MinValue;
                    tarea.EarlyFinish = DateTime.MinValue;
                }
            }
            var pendientes = new Queue<Tarea>(TareasSinDependencia());

            while (pendientes.Count > 0)
            {
                var tarea = pendientes.Dequeue();

                if (tarea.EstaCompleta())
                {
                    tarea.EarlyFinish = tarea.EarlyStart + tarea.Duracion;
                }
                else
                {
                    DateTime nuevoStart;
                    if(!tarea.TareasDependencia.Any())
                    {
                        nuevoStart = tarea.FechaInicio;
                    }
                    else
                    { 
                        nuevoStart = tarea.TareasDependencia.Max(d=>d.EarlyFinish);
                    }

                    if (tarea.EstaBloqueada() && tarea.RecursosForzados)
                    {
                        nuevoStart = ObtenerProximoInicioConRecursos(tarea, asignaciones, nuevoStart);
                    }

                    tarea.EarlyStart  = nuevoStart;
                    tarea.EarlyFinish = nuevoStart + tarea.Duracion;
                }

                foreach (var suc in tarea.TareasSucesoras)
                {
                    if (TodasLasDependenciasFueronProcesadas(suc))
                        pendientes.Enqueue(suc);
                }
            }
        }

    
        private DateTime ObtenerProximoInicioConRecursos(
            Tarea tarea,
            IEnumerable<AsignacionRecursoTarea> todasAsignaciones,
            DateTime desde)
        {
            DateTime posible = desde;
            var asigns = todasAsignaciones.Where(a => a.Tarea == tarea);

            foreach (var asign in asigns)
            {
                var recurso = asign.Recurso;
                int capacidad = recurso.CantidadDelRecurso;
                int usoInicial = recurso.CantidadEnUso;

                bool necesitaReajuste;
                do
                {
                    // Tareas que solapan en 'posible'
                    List<AsignacionRecursoTarea> solapantes = todasAsignaciones
                        .Where(a =>
                            a.Recurso == recurso
                            && a.Tarea.EarlyStart <= posible
                            && a.Tarea.EarlyFinish > posible)
                        .ToList();

                    int usoConcurrente = solapantes.Sum(a => a.CantidadNecesaria);
                    int usoTotal = usoInicial + usoConcurrente + asign.CantidadNecesaria;

                    if (usoTotal <= capacidad)
                    {
                        necesitaReajuste = false;
                    }
                    else
                    {
                        posible = solapantes.Min(a => a.Tarea.EarlyFinish);
                        necesitaReajuste = true;
                    }
                }
                while (necesitaReajuste);
            }

            return posible;
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
    public List<Tarea> CalcularRutaCritica(IEnumerable<AsignacionRecursoTarea> asignaciones)
    {
        if (!TareasAsociadas.Any())
            return new List<Tarea>();

        CalcularTiemposTempranos(asignaciones);

        CalcularTiemposTardios();

        foreach (var tarea in TareasAsociadas)
        {
            tarea.Holgura   = tarea.LateStart - tarea.EarlyStart;
            tarea.EsCritica = tarea.Holgura == TimeSpan.Zero;
        }

        CalcularFinEstimado();

        return TareasAsociadas
            .Where(t => t.EsCritica)
            .ToList();
    }

    public void AsignarUsuarioATarea(Usuario usuario, Tarea tarea)
    {
        if (!AsignacionesDelProyecto.Any(a => a.Usuario.Id == usuario.Id))
            throw new ArgumentException("El usuario no pertenece al proyecto");

        if (!TareasAsociadas.Contains(tarea))
            throw new ArgumentException("La tarea no pertenece al proyecto");

        if (tarea.UsuariosAsignados.Contains(usuario))
            throw new ArgumentException("El usuario ya está asignado a esta tarea");

        tarea.AgregarUsuario(usuario);
        var notificacion = new Notificacion($"Ha sido agregado a la tarea {tarea.Titulo}.");
        notificacion.AgregarUsuario(usuario);
    }

    
    // public void AsignarAdmin(Usuario usuario)
    // {
    //     Admin = usuario;
    //     Notificacion notificacion = new Notificacion("Eres administrador del proyecto " + Nombre + ".");
    //     notificacion.AgregarUsuario(usuario);
    // }
    
    // public bool EsAdmin(Usuario usuario)
    // {
    //     return usuario.Equals(Admin);
    // }
    
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

    public List<Tarea> TareasDelUsuario(Usuario usuario)
    {
        return TareasAsociadas.Where(t => t.UsuariosAsignados.Contains(usuario)).ToList();
    }
}