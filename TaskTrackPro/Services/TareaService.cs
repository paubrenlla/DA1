using DTOs;
using Domain;
using Domain.Enums;
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
        
        public void EliminarTarea(int tareaId)
        {
            TareaDTO dto = new TareaDTO { Id = tareaId };
            if (TieneSucesoras(dto))
                throw new InvalidOperationException("No se puede eliminar: la tarea tiene sucesoras.");
            if (TieneDependencias(dto))
                throw new InvalidOperationException("No se puede eliminar: la tarea tiene dependencias.");
            Tarea tarea = _tareaRepo.GetById(tareaId);
            Proyecto proyecto = tarea.Proyecto;
            proyecto.eliminarTarea(tarea);
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
            Usuario usuario = _usuarioRepo.GetById(miembroId);
            List<Tarea> TareasDeProyectoYUsuario = _tareaRepo.GetTareasDeUsuarioEnProyecto(miembroId, proyectoId);
    
            foreach (Tarea tarea in TareasDeProyectoYUsuario)
            {
                tarea.UsuariosAsignados.Remove(tarea.UsuariosAsignados.First(u => u.Id == miembroId));
            }
        }
    }
}
