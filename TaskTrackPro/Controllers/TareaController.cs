using DTOs;
using Services;
using System.Collections.Generic;
using Domain.Enums;
using IDataAcces;

namespace Controllers
{
    public class TareaController
    {
        private readonly ITareaService _service;

        public TareaController(ITareaService service)
        {
            _service = service;
        }

        public TareaDTO BuscarTareaPorId(int id)
        {
            return _service.BuscarTareaPorId(id);
        }

        public List<TareaDTO> ListarTareasPorProyecto(int proyectoId)
        {
            return _service.ListarTareasPorProyecto(proyectoId);
        }

        public TareaDTO CrearTarea(int proyectoId, TareaDTO dto)
        {
            return _service.CrearTarea(proyectoId, dto);
        }

        public void ModificarTarea(int tareaId, TareaDTO dto)
        {
            _service.ModificarTarea(tareaId, dto);
        }

        public void MarcarComoEjecutandose(int tareaId)
        {
            _service.MarcarComoEjecutandose(tareaId);
        }

        public void MarcarComoCompletada(int tareaId)
        {
            _service.MarcarComoCompletada(tareaId);
        }

        public void AgregarDependencia(int tareaId, int dependenciaId, int proyectoId)
        {
            _service.AgregarDependencia(tareaId, dependenciaId, proyectoId);
        }

        public void AgregarUsuario(int tareaId, int usuarioId)
        {
            _service.AgregarUsuario(tareaId, usuarioId);
        }

        public bool TieneSucesoras(TareaDTO tarea)
        {
            return _service.TieneSucesoras(tarea);
        }

        public bool UsuarioPerteneceALaTarea(int usuarioDtoId, int tareaId)
        {
            return _service.UsuarioPerteneceALaTarea(usuarioDtoId, tareaId);
        }

        public void EliminarTarea(int proyectoId, int tareaId)
        {
            _service.EliminarTarea( proyectoId, tareaId);
        }

        public TipoEstadoTarea GetEstadoTarea(int tareaId)
        {
            return _service.GetEstadoTarea(tareaId);
        }

        public List<UsuarioDTO>? ListarUsuariosDeTarea(int tareaId)
        {
            return _service.ListarUsuariosDeTarea(tareaId);
        }

        public void EliminarMiembroDeTarea(int miembroId, int idTarea)
        {
            _service.EliminarUsuarioDeTarea(miembroId, idTarea);
        }

        public void EliminarAUsuarioDeTareasDeProyecto(int miembroId, int proyectoId)
        {
            _service.EliminarUsuarioDeTareasDeProyecto(miembroId, proyectoId);
        }

        public List<TareaDTO>? ObtenerDependenciasDeTarea(int tareaId)
        {
            return _service.ObtenerDependenciasDeTarea(tareaId);
        }

        public void EliminarDependencia(int tareaId, int dependenciaId, int proyectoId)
        {
            _service.EliminarDependencia(tareaId, dependenciaId, proyectoId);
        }

        public List<TareaDTO> ListarTareasDelUsuario(int usuarioId, int proyectoId)
        {
            return _service.ListarTareasDelUsuario(usuarioId, proyectoId);
        }

        public bool PuedeCambiarDeEstado(int tareaSeleccionadaId)
        {
            return _service.PuedeCambiarDeEstado(tareaSeleccionadaId);
        }

        public List<TareaDTO>? ObtenerTareasParaAgregarDependencia(int tareaSeleccionadaId, int proyectoId)
        {
            return _service.ObtenerTareasParaAgregarDependencia(tareaSeleccionadaId, proyectoId);
        }

        public bool PuedeAgregarDependencias(int tareaSeleccionadaId)
        {
            return _service.PuedeAgregarDependencias(tareaSeleccionadaId);
        }

        public bool TieneDependencias(TareaDTO tarea)
        {
            return _service.TieneDependencias(tarea);
        }

        public bool PuedeEliminarTarea(TareaDTO tarea)
        {
            return _service.PuedeEliminarTarea(tarea);
        }

        public void ActualizarEstadoTarea(TipoEstadoTarea estado, TareaDTO tareaSeleccionada)
        {
            _service.ActualizarEstadoTarea(estado, tareaSeleccionada);
        }
    }
}