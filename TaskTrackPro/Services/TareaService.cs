using DTOs;
using Domain;
using Domain.Enums;
using IDataAcces;
using Services.Observers;

namespace Services
{
    public class TareaService : ITareaService
    {
        private readonly IDataAccessTarea _tareaRepo;
        private readonly IDataAccessProyecto _proyectoRepo;
        private readonly IDataAccessUsuario _usuarioRepo;

        public TareaService(
            IDataAccessTarea tareaRepo,
            IDataAccessProyecto proyectoRepo,
            IDataAccessUsuario usuarioRepo)
        {
            _tareaRepo = tareaRepo;
            _proyectoRepo = proyectoRepo;
            _usuarioRepo = usuarioRepo;
        }

        public TareaDTO BuscarTareaPorId(int id)
        {
            Tarea tarea = _tareaRepo.GetById(id);
            return Convertidor.ATareaDTO(tarea);
        }

        public List<TareaDTO> ListarTareasPorProyecto(int proyectoId)
        {
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);
            List<TareaDTO> dtoList = new List<TareaDTO>();

            foreach (Tarea tarea in proyecto.TareasAsociadas)
            {
                dtoList.Add(Convertidor.ATareaDTO(tarea));
            }

            return dtoList;
        }

        public TareaDTO CrearTarea(int proyectoId, TareaDTO dto)
        {
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);
            Tarea nuevaTarea = new Tarea(
                dto.Titulo,
                dto.Descripcion,
                dto.FechaInicio,
                dto.Duracion,
                esCritica: false);
            if (dto.FechaInicio <= proyecto.FechaInicio)
                throw new ArgumentException("La tarea no puede tener fecha de inicio menor al proyecto");
            proyecto.TareasAsociadas.Add(nuevaTarea);

            _tareaRepo.Add(nuevaTarea);
            proyecto.CalcularRutaCritica();

            return Convertidor.ATareaDTO(nuevaTarea);
        }

        public void ModificarTarea(int tareaId, TareaDTO dto)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            tarea.Modificar(
                dto.Titulo,
                dto.Descripcion,
                dto.FechaInicio,
                dto.Duracion);
        }

        public void MarcarComoEjecutandose(int tareaId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            tarea.MarcarTareaComoEjecutandose();
        }

        public void MarcarComoCompletada(int tareaId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            tarea.MarcarTareaComoCompletada();
        }

        public void AgregarDependencia(int tareaId, int dependenciaId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            Tarea dependencia = _tareaRepo.GetById(dependenciaId);
            tarea.AgregarDependencia(dependencia);
            _tareaRepo.Update(tarea);
            _tareaRepo.Update(dependencia);
        }

        public void AgregarUsuario(int tareaId, int usuarioId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            Usuario usuario = _usuarioRepo.GetById(usuarioId);
            tarea.AgregarUsuario(usuario);
            _tareaRepo.Update(tarea);
        }

        public bool TieneSucesoras(TareaDTO tareaDto)
        {
            Tarea tarea = _tareaRepo.GetById(tareaDto.Id);
            return tarea.TareasSucesoras.Count != 0;
        }

        public bool UsuarioPerteneceALaTarea(int usuarioDtoId, int tareaId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            return tarea.UsuariosAsignados.Any(usuario => usuario.Id == usuarioDtoId);
        }
        
        public bool TieneDependencias(TareaDTO tareaDto)
        {
            Tarea tarea = _tareaRepo.GetById(tareaDto.Id);
            return tarea.TareasDependencia.Count != 0;
        }
        
        public void EliminarTarea(int proyectoId, int tareaId)
        {
            var dto = new TareaDTO { Id = tareaId };
            if (TieneSucesoras(dto))
                throw new InvalidOperationException("No se puede eliminar: la tarea tiene sucesoras.");
            if (TieneDependencias(dto))
                throw new InvalidOperationException("No se puede eliminar: la tarea tiene dependencias.");

            Tarea tarea = _tareaRepo.GetById(tareaId);
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);
            proyecto.TareasAsociadas.Remove(tarea);
            _tareaRepo.Remove(tarea);
        }

        public TipoEstadoTarea GetEstadoTarea(int tareaId)
        {
            return _tareaRepo.GetById(tareaId).EstadoActual.Valor;
        }
        
        public List<UsuarioDTO>? ListarUsuariosDeTarea(int tareaID)
        {
            Tarea tarea = _tareaRepo.GetById(tareaID);
            return tarea?.UsuariosAsignados.Select(Convertidor.AUsuarioDTO).ToList();
        }

        public void EliminarUsuarioDeTarea(int miembroId, int idTarea)
        {
            Tarea tarea = _tareaRepo.GetById(idTarea);
    
            Usuario usuario = _usuarioRepo.GetById(miembroId);
    
            tarea.UsuariosAsignados.Remove(usuario);
        }

        public void EliminarUsuarioDeTareasDeProyecto(int miembroId, int proyectoId)
        {
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);
            Usuario usuario   = _usuarioRepo.GetById(miembroId);

            foreach (Tarea tarea in proyecto.TareasAsociadas
                         .Where(t => t.UsuariosAsignados.Any(u => u.Id == miembroId)))
            {
                tarea.UsuariosAsignados.Remove(usuario);
            }
        }

        public List<TareaDTO>? ObtenerDependenciasDeTarea(int tareaId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            if (tarea?.TareasDependencia == null || !tarea.TareasDependencia.Any())
                return new List<TareaDTO>();

            return tarea.TareasDependencia.Select(Convertidor.ATareaDTO).ToList();
        }

        public void EliminarDependencia(int tareaId, int dependenciaId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            Tarea dependencia = _tareaRepo.GetById(dependenciaId);
            
            if (tarea.TareasDependencia.Contains(dependencia))
            {
                tarea.TareasDependencia.Remove(dependencia);
                dependencia.TareasSucesoras.Remove(tarea);

                tarea.ActualizarEstado();
            }
        }

        public List<TareaDTO> ListarTareasDelUsuario(int usuarioId, int proyectoId)
        {
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);

            List<TareaDTO> tareasDelUsuario = proyecto.TareasAsociadas
                .Where(t => t.UsuariosAsignados.Any(u => u.Id == usuarioId))
                .Select(Convertidor.ATareaDTO)
                .ToList();

            return tareasDelUsuario;
        }

        public bool PuedeCambiarDeEstado(int tareaSeleccionadaId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaSeleccionadaId);
            TipoEstadoTarea estado = tarea.EstadoActual.Valor;
            
            return estado == TipoEstadoTarea.Pendiente || estado == TipoEstadoTarea.Ejecutandose;
        }

        public List<TareaDTO>? ObtenerTareasParaAgregarDependencia(int tareaSeleccionadaId, int proyectoId)
        {
            Tarea tareaActual = _tareaRepo.GetById(tareaSeleccionadaId);
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);

            if (proyecto == null)
                return new List<TareaDTO>();

            List<TareaDTO> tareasDisponibles = proyecto.TareasAsociadas
                .Where(t => t.Id != tareaSeleccionadaId && 
                            !tareaActual.TareasDependencia.Contains(t) &&
                            !tareaActual.TareasSucesoras.Contains(t))
                .Select(Convertidor.ATareaDTO)
                .ToList();

            return tareasDisponibles;
        }

        public bool PuedeAgregarDependencias(int tareaSeleccionadaId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaSeleccionadaId);
            TipoEstadoTarea estado = tarea.EstadoActual.Valor;
            
            // Solo puede agregar dependencias si está en Pendiente o Bloqueada
            return estado == TipoEstadoTarea.Pendiente || estado == TipoEstadoTarea.Bloqueada;
        }

        public bool PuedeEliminarTarea(TareaDTO tarea)
        {
            return !TieneDependencias(tarea) && !TieneSucesoras(tarea);
        }

        public void ActualizarEstadoTarea(TipoEstadoTarea estado, TareaDTO tareaSeleccionada)
        {
            if (estado == TipoEstadoTarea.Pendiente)
            {
                MarcarComoEjecutandose(tareaSeleccionada.Id);
            }
            else if (estado == TipoEstadoTarea.Ejecutandose)
            {
                MarcarComoCompletada(tareaSeleccionada.Id);
            }
        }
    }
}
