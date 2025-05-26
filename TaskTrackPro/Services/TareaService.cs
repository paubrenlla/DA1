using BusinessLogic;
using IDataAcces;
using Services.DTOs;

namespace Services
{
    public class TareaService
    {
        private readonly IDataAccessTarea _repoTareas;
        private readonly IDataAccessProyecto _repoProyectos;
        private readonly IDataAccessUsuario _repoUsuarios;

        public TareaService(IDataAccessTarea t, IDataAccessProyecto p, IDataAccessUsuario u)
        {
            _repoTareas = t;
            _repoProyectos = p;
            _repoUsuarios = u;
        }

        public TareaDTO BuscarTareaPorId(int id)
        {
            var tarea = _repoTareas.GetById(id);
            return Convertidor.ATareaDTO(tarea);
        }

        public List<TareaDTO> ListarTareasPorProyecto(int proyectoId)
        {
            var proyecto = _repoProyectos.GetById(proyectoId);

            return proyecto.TareasAsociadas
                .Select(t => Convertidor.ATareaDTO(t))
                .ToList();
        }
        
        public TareaDTO CrearTarea(int proyectoId, TareaDTO dto)
        {
            var proyecto = _repoProyectos.GetById(proyectoId);
            if (proyecto == null)
                throw new ArgumentException("Proyecto no encontrado");

            var nuevaTarea = new Tarea(dto.Titulo, dto.Descripcion, dto.FechaInicio, dto.Duracion, esCritica: false);

            nuevaTarea.Proyecto = proyecto;
            proyecto.TareasAsociadas.Add(nuevaTarea);

            _repoTareas.Add(nuevaTarea);

            proyecto.CalcularRutaCritica();

            return Convertidor.ATareaDTO(nuevaTarea);
        }
        
        public void ModificarTarea(int tareaId, TareaDTO dto)
        {
            var tarea = _repoTareas.GetById(tareaId);
            tarea.Modificar(dto.Titulo, dto.Descripcion, dto.FechaInicio, dto.Duracion);
        }
        
        public void MarcarComoEjecutandose(int tareaId)
        {
            var tarea = _repoTareas.GetById(tareaId);
            tarea.MarcarTareaComoEjecutandose();
        }
        
        public void MarcarComoCompletada(int tareaId)
        {
            var tarea = _repoTareas.GetById(tareaId);
            tarea.MarcarTareaComoCompletada();
        }
        
        public void AgregarDependencia(int tareaId, int dependenciaId)
        {
            var tarea = _repoTareas.GetById(tareaId);
            var dependencia = _repoTareas.GetById(dependenciaId);
            tarea.AgregarDependencia(dependencia);
        }
        
        public void AgregarUsuario(int tareaId, int usuarioId)
        {
            var tarea = _repoTareas.GetById(tareaId);
            var usuario = _repoUsuarios.GetById(usuarioId);
            tarea.AgregarUsuario(usuario);
        }


//TODO recursos de las tareas cuadno haya RecursoService



    }
}