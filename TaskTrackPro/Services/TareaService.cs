﻿using DTOs;
using Domain;
using Domain.Enums;
using Domain.Observers;
using IDataAcces;
using Services.Observers;
using IServices;

namespace Services
{
    public class TareaService : ITareaService
    {
        private readonly IDataAccessTarea _tareaRepo;
        private readonly IDataAccessProyecto _proyectoRepo;
        private readonly IDataAccessUsuario _usuarioRepo;
        
        private readonly IAsignacionRecursoTareaService _asignacionService;
        private readonly IRecursoService _recursoService;
        
        private readonly IEnumerable<ITareaObserver> _observers;
        private readonly NotificadorTarea _notificador;
        private readonly IProyectoService _proyectoService;

        public TareaService(
            IDataAccessTarea tareaRepo,
            IDataAccessProyecto proyectoRepo,
            IDataAccessUsuario usuarioRepo,
            IAsignacionRecursoTareaService asignacionService,
            IRecursoService recursoService,
            IEnumerable<ITareaObserver> observers,
            IProyectoService proyectoService)
        {
            _tareaRepo = tareaRepo;
            _proyectoRepo = proyectoRepo;
            _usuarioRepo = usuarioRepo;
            _asignacionService = asignacionService;
            _recursoService = recursoService;
            _observers = observers;
            _proyectoService = proyectoService;
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
            if (dto.FechaInicio < proyecto.FechaInicio)
                throw new ArgumentException("La tarea no puede tener fecha de inicio menor al proyecto");
            proyecto.TareasAsociadas.Add(nuevaTarea);

            _tareaRepo.Add(nuevaTarea);
            _proyectoService.ObtenerRutaCritica(proyectoId);
            _tareaRepo.Update(nuevaTarea);
            
            foreach (ITareaObserver obs in _observers)
            {
                obs.TareaAgregada(proyecto, nuevaTarea);
            }

            return Convertidor.ATareaDTO(nuevaTarea);
        }

        public void ModificarTarea(int tareaId, TareaDTO dto, int proyectoId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            DateTime fechaAnterior= tarea.FechaInicio;
            TimeSpan duracionAnterior = tarea.Duracion;
            tarea.Modificar(
                dto.Titulo,
                dto.Descripcion,
                dto.FechaInicio,
                dto.Duracion);
            _tareaRepo.Update(tarea);
            if (fechaAnterior != tarea.FechaInicio || duracionAnterior != tarea.Duracion)
            {
                Proyecto proyecto = _proyectoRepo.GetById(proyectoId);
                _proyectoService.ObtenerRutaCritica(proyectoId);
                foreach (ITareaObserver obs in _observers)
                {
                    obs.TareaModificada(proyecto, tarea);
                }
            }
        }

        public void MarcarComoEjecutandose(int tareaId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            if (_asignacionService.VerificarRecursosDeTareaDisponibles(tareaId) && tarea.VerificarDependenciasCompletadas())
            {
                tarea.MarcarTareaComoEjecutandose();
                _tareaRepo.Update(tarea);
                
                List<AsignacionRecursoTareaDTO> recursosDeLaTarea = _asignacionService.GetAsignacionesDeTarea(tareaId);

                foreach (AsignacionRecursoTareaDTO asignacion in recursosDeLaTarea)
                {
                    _recursoService.ConsumirRecurso(asignacion.Recurso.Id, asignacion.Cantidad);
                }
            }
            tarea.MarcarTareaComoEjecutandose();
            _tareaRepo.Update(tarea);
        }

        public void MarcarComoCompletada(int tareaId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            tarea.MarcarTareaComoCompletada();
            _tareaRepo.Update(tarea);
            
            List<AsignacionRecursoTareaDTO> recursosDeLaTarea = _asignacionService.GetAsignacionesDeTarea(tareaId);
            _asignacionService.GetAsignacionesDeTarea(tareaId);

            foreach (AsignacionRecursoTareaDTO asignacion in recursosDeLaTarea)
            {
                _recursoService.LiberarRecurso(asignacion.Recurso.Id, asignacion.Cantidad);
            }
        }

        public void AgregarDependencia(int tareaId, int dependenciaId, int proyectoId)
        {
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);
            Tarea tarea = _tareaRepo.GetById(tareaId);
            Tarea dependencia = _tareaRepo.GetById(dependenciaId);
            tarea.AgregarDependencia(dependencia);
            
            _tareaRepo.Update(tarea);
            _tareaRepo.Update(dependencia);

            _proyectoService.ObtenerRutaCritica(proyectoId);
            foreach (ITareaObserver obs in _observers)
            {
                obs.ModificacionDependencias(proyecto, tarea);
            }
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
            TareaDTO dto = new TareaDTO { Id = tareaId };
            if (TieneSucesoras(dto))
                throw new InvalidOperationException("No se puede eliminar: la tarea tiene sucesoras.");
            if (TieneDependencias(dto))
                throw new InvalidOperationException("No se puede eliminar: la tarea tiene dependencias.");

            Tarea tarea = _tareaRepo.GetById(tareaId);
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);
            proyecto.TareasAsociadas.Remove(tarea);
            _tareaRepo.Remove(tarea);
            _proyectoService.ObtenerRutaCritica(proyectoId);
            _proyectoRepo.Update(proyecto);
            foreach (ITareaObserver obs in _observers)
            {
                obs.TareaEliminada(proyecto, tarea);
            }
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
            _tareaRepo.Update(tarea);
        }

        public void EliminarUsuarioDeTareasDeProyecto(int miembroId, int proyectoId)
        {
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);
            Usuario usuario   = _usuarioRepo.GetById(miembroId);

            foreach (Tarea tarea in proyecto.TareasAsociadas
                         .Where(t => t.UsuariosAsignados.Any(u => u.Id == miembroId)))
            {
                tarea.UsuariosAsignados.Remove(usuario);
                _tareaRepo.Update(tarea);
            }
        }

        public List<TareaDTO>? ObtenerDependenciasDeTarea(int tareaId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            if (tarea?.TareasDependencia == null || !tarea.TareasDependencia.Any())
                return new List<TareaDTO>();

            return tarea.TareasDependencia.Select(Convertidor.ATareaDTO).ToList();
        }

        public void EliminarDependencia(int tareaId, int dependenciaId, int proyectoId)
        {
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);
            Tarea tarea = _tareaRepo.GetById(tareaId);
            Tarea dependencia = _tareaRepo.GetById(dependenciaId);
            
            if (tarea.TareasDependencia.Contains(dependencia))
            {
                tarea.EliminarDependencia(dependencia);
                dependencia.EliminarSucesora(tarea);

                tarea.ActualizarEstado();
            }
            
            _tareaRepo.Update(tarea);
            _tareaRepo.Update(dependencia);
            _proyectoService.ObtenerRutaCritica(proyectoId);
            
            foreach (ITareaObserver obs in _observers)
            {
                obs.ModificacionDependencias(proyecto, tarea);
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
            Tarea tarea = _tareaRepo.GetById(tareaSeleccionada.Id);
            _tareaRepo.Update(tarea);
        }

        public bool PuedeForzarRecursos(TareaDTO dto)
        {
            Tarea tarea = _tareaRepo.GetById(dto.Id);
            return tarea.DependenciasEfectuadas() && !tarea.RecursosForzados && tarea.EstadoActual.Valor == TipoEstadoTarea.Bloqueada;
        }

        public void ForzarRecursos(int proyectoId, int tareaId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            Proyecto proyecto = _proyectoRepo.GetById(proyectoId);
            tarea.RecursosForzados = true;
            _tareaRepo.Update(tarea);
            _proyectoService.ObtenerRutaCritica(proyectoId);
            _proyectoRepo.Update(proyecto);
            foreach (ITareaObserver obs in _observers)
            {
                obs.SeForzaronRecursos(proyecto, tarea);
            }
        }
    }
}
