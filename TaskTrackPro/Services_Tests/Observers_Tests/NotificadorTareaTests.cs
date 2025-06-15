using Domain;
using IDataAcces;
using Moq;
using Services.Observers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Services_Tests.Observers_Tests
{
    [TestClass]
    public class NotificadorTareaTests
    {
        private Mock<IDataAccessNotificacion> _mockNotifRepo;
        private Mock<IDataAccessAsignacionProyecto> _mockAsignacionRepo;
        private NotificadorTarea _observer;

        private Proyecto _proyecto;
        private Tarea _tarea;
        private Usuario _adminUsuario;
        private AsignacionProyecto _adminAsignacion;

        [TestInitialize]
        public void SetUp()
        {
            _mockNotifRepo = new Mock<IDataAccessNotificacion>();
            _mockAsignacionRepo = new Mock<IDataAccessAsignacionProyecto>();

            _proyecto = new Proyecto("Proyecto X", "Desc", DateTime.Today);
            _tarea = new Tarea("Tarea Y", "Desc", DateTime.Today, TimeSpan.FromDays(1), false);
            _proyecto.TareasAsociadas.Add(_tarea);

            _adminUsuario = new Usuario("admin@x.com", "Admin", "User", "Contraseña1!", DateTime.Today);
            _adminAsignacion = new AsignacionProyecto
            {
                Proyecto = _proyecto,
                Usuario = _adminUsuario,
                Rol = Domain.Enums.Rol.Administrador
            };

            _mockAsignacionRepo
                .Setup(r => r.GetAdminProyecto(_proyecto.Id))
                .Returns(_adminAsignacion);

            _observer = new NotificadorTarea(
                _mockNotifRepo.Object,
                _mockAsignacionRepo.Object
            );
        }

        [TestMethod]
        public void TareaEliminadaLlamaAddConNotificacionCorrecta()
        {
            string esperadoMensaje =
                $"La tarea '{_tarea.Titulo}' fue eliminada del proyecto '{_proyecto.Nombre}'.\n" +
                $"Esto puede cambiar la fecha de fin del proyecto!!!";
            _observer.TareaEliminada(_proyecto, _tarea);

            _mockNotifRepo.Verify(repo => repo.Add(
                    It.Is<Notificacion>(n =>
                        n.Mensaje == esperadoMensaje &&
                        n.UsuariosNotificados.Contains(_adminUsuario)
                    )),
                Times.Once);
        }
        
        [TestMethod]
        public void TareaAgregadaLlamaAddConNotificacionCorrecta()
        {
            string esperadoMensaje =
                $"Se ha agregado la tarea '{_tarea.Titulo}' al proyecto '{_proyecto.Nombre}'.\n"+
                $"Esto puede cambiar la fecha de fin del proyecto!!!";
           
            _observer.TareaAgregada(_proyecto, _tarea);

            _mockNotifRepo.Verify(repo => repo.Add(
                    It.Is<Notificacion>(n =>
                        n.Mensaje == esperadoMensaje
                        && n.UsuariosNotificados.Contains(_adminUsuario)
                    )),
                Times.Once);
        }
        
        [TestMethod]
        public void ModificarDependenciaLlamaAddConNotificacionCorrecta()
        {
            string esperadoMensaje =
                $"Se ha modificado una dependencia en la tarea '{_tarea.Titulo}' en el proyecto '{_proyecto.Nombre}'.\n"+
                $"Es muy probable que la fecha de fin del proyecto haya cambiado!!!";
           
            _observer.ModificacionDependencias(_proyecto, _tarea);

            _mockNotifRepo.Verify(repo => repo.Add(
                    It.Is<Notificacion>(n =>
                        n.Mensaje == esperadoMensaje
                        && n.UsuariosNotificados.Contains(_adminUsuario)
                    )),
                Times.Once);
        }

    }
}
