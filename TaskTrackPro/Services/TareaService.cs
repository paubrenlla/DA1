using DTOs;
using Domain;
using IDataAcces;

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

            nuevaTarea.Proyecto = proyecto;
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
        }

        public void AgregarUsuario(int tareaId, int usuarioId)
        {
            Tarea tarea = _tareaRepo.GetById(tareaId);
            Usuario usuario = _usuarioRepo.GetById(usuarioId);
            tarea.AgregarUsuario(usuario);
        }
    }
}
