
using DataAccess;
using Domain;
using Domain.Enums;
using DTOs;
using IDataAcces;
using Services;

namespace Services_Tests
{
    [TestClass]
    public class ProyectoServiceTests
    {
        private ProyectoService _service;
        private IDataAccessProyecto _repoProyectos;
        private IDataAccessUsuario _repoUsuarios;
        private IDataAccessAsignacionProyecto _repoAsignaciones;
        private Usuario _usuario1;
        private Usuario _usuario2;
        private Proyecto _proyecto1;
        private Proyecto _proyecto2;

        [TestInitialize]
        public void SetUp()
        {
            _repoProyectos = new ProyectoDataAccess();
            _repoUsuarios = new UsuarioDataAccess();
            _repoAsignaciones = new AsignacionProyectoDataAccess();
            _service = new ProyectoService(_repoProyectos, _repoUsuarios, _repoAsignaciones);

            _usuario1 = new Usuario("u1@test.com", "Nombre1", "Apellido1", "Contraseña1!", new DateTime(1990,1,1));
            _usuario2 = new Usuario("u2@test.com", "Nombre2", "Apellido2", "Contraseña1!", new DateTime(1991,1,1));
            _repoUsuarios.Add(_usuario1);
            _repoUsuarios.Add(_usuario2);

            _proyecto1 = new Proyecto("P1", "Desc1", DateTime.Today.AddDays(1));
            _proyecto2 = new Proyecto("P2", "Desc2", DateTime.Today.AddDays(2));
            _repoProyectos.Add(_proyecto1);
            _repoProyectos.Add(_proyecto2);
        }

        [TestMethod]
        public void GetByIdDevuelveDTOBien()
        {
            ProyectoDTO dto = _service.GetById(_proyecto1.Id);
            Assert.AreEqual(_proyecto1.Id, dto.Id);
            Assert.AreEqual(_proyecto1.Nombre, dto.Nombre);
            Assert.AreEqual(_proyecto1.Descripcion, dto.Descripcion);
        }

        [TestMethod]
        public void GetAllDevuelveLaListaDeDTOs()
        {
            List<ProyectoDTO> list = _service.GetAll();
            Assert.AreEqual(2, list.Count);
            
            Assert.AreEqual(_proyecto1.Id, list[0].Id);
            Assert.AreEqual(_proyecto2.Id, list[1].Id);
        }

        [TestMethod]
        public void CrearProyectoCreaElProyectoCorrectamenteYLoGuarda()
        {
            ProyectoDTO dtoIn = new ProyectoDTO { Nombre = "Nuevo", Descripcion = "NuevaDesc", FechaInicio = DateTime.Today.AddDays(5) };
            ProyectoDTO proyectoCreadoDto = _service.CrearProyecto(dtoIn);
            Assert.IsNotNull(proyectoCreadoDto);
            Assert.AreEqual(dtoIn.Nombre, proyectoCreadoDto.Nombre);
            Proyecto proyectoCreado = _repoProyectos.GetById(proyectoCreadoDto.Id);
            Assert.IsNotNull(proyectoCreado);
            Assert.AreEqual(dtoIn.Nombre, proyectoCreado.Nombre);
            Assert.AreEqual(dtoIn.Descripcion, proyectoCreado.Descripcion);
            Assert.AreEqual(dtoIn.FechaInicio, proyectoCreado.FechaInicio);
        }

        [TestMethod]
        public void ModificarProyectoCorrectamente()
        {
            ProyectoDTO dto = new ProyectoDTO { Id = _proyecto1.Id, Nombre = _proyecto1.Nombre, Descripcion = "Modificada", FechaInicio = DateTime.Today.AddDays(10) };
            _service.ModificarProyecto(dto);
            Proyecto proyectoModificado = _repoProyectos.GetById(_proyecto1.Id);
            Assert.AreEqual("Modificada", proyectoModificado.Descripcion);
            Assert.AreEqual(dto.FechaInicio, proyectoModificado.FechaInicio);
        }

        [TestMethod]
        public void DeleteBorraElProyectoYSusAsignaciones()  
        {
            AsignacionProyecto asign = new AsignacionProyecto( _proyecto1,_usuario1, Rol.Miembro);
            _repoAsignaciones.Add(asign);
            Assert.IsTrue(_repoAsignaciones.UsuariosDelProyecto(_proyecto1.Id).Any());

            _service.Delete(_proyecto1.Id);
            Assert.IsFalse(_repoAsignaciones.UsuariosDelProyecto(_proyecto1.Id).Any());
        }

        [TestMethod]
        public void UsuarioEsAdminDeAlgunProyectoDevuelveTrueSiEsAsi()
        {
            AsignacionProyecto asignAdmin = new AsignacionProyecto(_proyecto2, _usuario2, Rol.Administrador);
            _repoAsignaciones.Add(asignAdmin);

            Assert.IsTrue(_service.UsuarioEsAdminDeAlgunProyecto(_usuario2.Id));
            Assert.IsFalse(_service.UsuarioEsAdminDeAlgunProyecto(_usuario1.Id));
        }

        [TestMethod]
        public void ProyectosDelUsuarioDevuelveDTOsDeProyectos()
        {
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario1, Rol.Miembro));
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto2, _usuario1, Rol.Miembro));
            List<ProyectoDTO> proyectosDelUsuario = _service.ProyectosDelUsuario(_usuario1.Id);
            Assert.AreEqual(2, proyectosDelUsuario.Count);
            Assert.AreEqual(_proyecto1.Id, proyectosDelUsuario[0].Id);
            Assert.AreEqual(_proyecto2.Id, proyectosDelUsuario[1].Id);
        }

        [TestMethod]
        public void EliminarAsignacionesDeUsuarioEliminaTodasLasAsignacionesConElUsuario()
        {
            AsignacionProyecto asignacion1 = new AsignacionProyecto(_proyecto1,_usuario1, Rol.Miembro);
            AsignacionProyecto asignacion2 = new AsignacionProyecto(_proyecto2,_usuario1, Rol.Miembro);
            _repoAsignaciones.Add(asignacion1);
            _repoAsignaciones.Add(asignacion2);
            Assert.AreEqual(2, _repoAsignaciones.AsignacionesDelUsuario(_usuario1.Id).Count());

            _service.EliminarAsignacionesDeUsuario(_usuario1.Id);
            Assert.AreEqual(0, _repoAsignaciones.AsignacionesDelUsuario(_usuario1.Id).Count());
        }

        [TestMethod]
        public void UsuarioEsAdminEnProyectoDevuelveTrueSiEsAdminDelProyecto()
        {
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto2, _usuario2, Rol.Administrador));
            Assert.IsTrue(_service.UsuarioEsAdminEnProyecto(_usuario2.Id, _proyecto2.Id));
            Assert.IsFalse(_service.UsuarioEsAdminEnProyecto(_usuario1.Id, _proyecto2.Id));
        }
        
        [TestMethod]
        public void AsignarAdminDeProyecto_ReemplazaCorrectamenteElAdministradorAnterior()
        {
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario1, Rol.Administrador));

            _service.AsignarAdminDeProyecto(_usuario2.Id, _proyecto1.Id);

            List<AsignacionProyecto> asignaciones = _repoAsignaciones.UsuariosDelProyecto(_proyecto1.Id).ToList();

            Assert.AreEqual(1, asignaciones.Count);
            Assert.AreEqual(_usuario2.Id, asignaciones[0].Usuario.Id);
            Assert.AreEqual(Rol.Administrador, asignaciones[0].Rol);
        }

    }
}
