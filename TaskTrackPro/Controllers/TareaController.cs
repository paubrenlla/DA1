using DTOs;
using Services;
using System.Collections.Generic;
using Domain.Enums;

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

        public void AgregarDependencia(int tareaId, int dependenciaId)
        {
            _service.AgregarDependencia(tareaId, dependenciaId);
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

        public void EliminarTarea(int tareaId)
        {
            _service.EliminarTarea(tareaId);
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
    }
}