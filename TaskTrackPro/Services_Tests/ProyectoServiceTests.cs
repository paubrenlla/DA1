using DataAccess;
using Domain;
using Domain.Enums;
using DTOs;
using IDataAcces;
using Microsoft.EntityFrameworkCore;
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
        private IDataAccessAsignacionRecursoTarea _asignacionRecurso;

        private IDataAccessTarea _repoTareas;

        private Usuario _usuario1;
        private Usuario _usuario2;
        private Proyecto _proyecto1;
        private Proyecto _proyecto2;
        private Tarea _t1;
        private Tarea _t2;
        private Tarea _t3;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new SqlContext(options);

            _repoUsuarios = new UsuarioDataAccess(context);
            _repoProyectos = new ProyectoDataAccess(context);
            _repoAsignaciones = new AsignacionProyectoDataAccess(context);
            _asignacionRecurso = new AsignacionRecursoTareaDataAccess(context);

            _service = new ProyectoService(_repoProyectos, _repoUsuarios, _repoAsignaciones, _asignacionRecurso);

            _usuario1 = new Usuario("u1@test.com", "Nombre1", "Apellido1", "Contraseña1!", new DateTime(1990, 1, 1));
            _usuario2 = new Usuario("u2@test.com", "Nombre2", "Apellido2", "Contraseña1!", new DateTime(1991, 1, 1));
            _repoUsuarios.Add(_usuario1);
            _repoUsuarios.Add(_usuario2);

            _proyecto1 = new Proyecto("P1", "Desc1", DateTime.Today.AddDays(1));
            _proyecto2 = new Proyecto("P2", "Desc2", DateTime.Today.AddDays(2));
            _repoProyectos.Add(_proyecto1);
            _repoProyectos.Add(_proyecto2);

            _t1 = new Tarea("T1", "D1", DateTime.Today.AddDays(1), TimeSpan.FromDays(2), esCritica: false);
            _t2 = new Tarea("T2", "D2", DateTime.Today.AddDays(3), TimeSpan.FromDays(1), esCritica: false);
            _t3 = new Tarea("T3", "D3", DateTime.Today.AddDays(4), TimeSpan.FromDays(2), esCritica: false);

            _proyecto1.TareasAsociadas.Add(_t1);
            _proyecto1.TareasAsociadas.Add(_t2);
            _proyecto1.TareasAsociadas.Add(_t3);

            _t2.AgregarDependencia(_t1);
            _t3.AgregarDependencia(_t2);
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
    
        [TestMethod]
        public void GetAdminDeProyecto_DevuelveElAdminCorrectamente()
        {
            AsignacionProyecto asignAdmin = new AsignacionProyecto(_proyecto2, _usuario2, Rol.Administrador);
            _repoAsignaciones.Add(asignAdmin);

            UsuarioDTO adminDTO = _service.GetAdminDeProyecto(_proyecto2.Id);

            Assert.IsNotNull(adminDTO);
            Assert.AreEqual(_usuario2.Id, adminDTO.Id);
            Assert.AreEqual(_usuario2.Nombre, adminDTO.Nombre);
            Assert.AreEqual(_usuario2.Apellido, adminDTO.Apellido);
            Assert.AreEqual(_usuario2.Email, adminDTO.Email);
        }
        
        [TestMethod]
        public void GetMiembrosDeProyecto_DevuelveListaCorrecta()
        {
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario1, Rol.Miembro));
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario2, Rol.Miembro));

            List<UsuarioDTO> miembros = _service.GetMiembrosDeProyecto(_proyecto1.Id)!;

            Assert.AreEqual(2, miembros.Count);
            Assert.IsTrue(miembros.Any(u => u.Id == _usuario1.Id));
            Assert.IsTrue(miembros.Any(u => u.Id == _usuario2.Id));
        }

        [TestMethod]
        public void AgregarMiembroProyecto_AgregaCorrectamenteUnMiembro()
        {
            _service.AgregarMiembroProyecto(_usuario1.Id, _proyecto1.Id);

            List<AsignacionProyecto> asignaciones = _repoAsignaciones.UsuariosDelProyecto(_proyecto1.Id);
            Assert.AreEqual(1, asignaciones.Count());
            AsignacionProyecto asignacion = asignaciones.First();
            Assert.AreEqual(_usuario1.Id, asignacion.Usuario.Id);
            Assert.AreEqual(Rol.Miembro, asignacion.Rol);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AgregarMiembroProyecto_UsuarioYaAsignado_LanzaExcepcion()
        {
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario1, Rol.Miembro));

            _service.AgregarMiembroProyecto(_usuario1.Id, _proyecto1.Id);
        }

        [TestMethod]
        public void EliminarMiembroDeProyecto_EliminaCorrectamenteUnMiembro()
        {
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario2, Rol.Administrador));
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario1, Rol.Miembro));

            _service.EliminarMiembroDeProyecto(_usuario1.Id, _proyecto1.Id);

            List<AsignacionProyecto> asignacionesRestantes = _repoAsignaciones.UsuariosDelProyecto(_proyecto1.Id);
            Assert.IsFalse(asignacionesRestantes.Any(a => a.Usuario.Id == _usuario1.Id));
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EliminarMiembroDeProyecto_NoPermiteEliminarAdmin_LanzaExcepcion()
        {
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario1, Rol.Administrador));

            _service.EliminarMiembroDeProyecto(_usuario1.Id, _proyecto1.Id);
        }
        
        [TestMethod]
        public void ObtenerRutaCriticaDevuelveSoloCriticas()
        {
            List<TareaDTO> criticas = _service.ObtenerRutaCritica(_proyecto1.Id);
            Assert.AreEqual(3, criticas.Count);
        }

        [TestMethod]
        public void TareasNoCriticasDevuelveVacioCuandoTodasSonCriticas()
        {
            List<TareaDTO> noCrit = _service.TareasNoCriticas(_proyecto1.Id);
            Assert.AreEqual(0, noCrit.Count);
        }

        [TestMethod]
        public void TareasOrdenadasPorInicio_OrdenCorrecto()
        {
            List<TareaDTO> orden = _service.TareasOrdenadasPorInicio(_proyecto1.Id);
            Assert.AreEqual(_t1.Id, orden[0].Id);
            Assert.AreEqual(_t2.Id, orden[1].Id);
            Assert.AreEqual(_t3.Id, orden[2].Id);
        }
        
        [TestMethod]
        public void AsignarLiderDeProyecto_AsignaCorrectamenteUnLider()
        {
            _service.AsignarLiderDeProyecto(_usuario1.Id, _proyecto1.Id);

            List<AsignacionProyecto> asignaciones = _repoAsignaciones.UsuariosDelProyecto(_proyecto1.Id);
            Assert.AreEqual(1, asignaciones.Count());
            AsignacionProyecto asignacion = asignaciones.First();
            Assert.AreEqual(_usuario1.Id, asignacion.Usuario.Id);
            Assert.AreEqual(Rol.Lider, asignacion.Rol);
        }

        [TestMethod]
        public void AsignarLiderDeProyecto_ReemplazaCorrectamenteElLiderAnterior()
        {
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario1, Rol.Lider));

            _service.AsignarLiderDeProyecto(_usuario2.Id, _proyecto1.Id);

            List<AsignacionProyecto> asignaciones = _repoAsignaciones.UsuariosDelProyecto(_proyecto1.Id).ToList();

            Assert.AreEqual(1, asignaciones.Count);
            Assert.AreEqual(_usuario2.Id, asignaciones[0].Usuario.Id);
            Assert.AreEqual(Rol.Lider, asignaciones[0].Rol);
        }

        [TestMethod]
        public void AsignarLiderDeProyecto_NoAfectaOtrosRoles()
        {
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario1, Rol.Administrador));
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario2, Rol.Miembro));

            _service.AsignarLiderDeProyecto(_usuario1.Id, _proyecto1.Id);

            List<AsignacionProyecto> asignaciones = _repoAsignaciones.UsuariosDelProyecto(_proyecto1.Id).ToList();
            
            Assert.AreEqual(3, asignaciones.Count);
            Assert.IsTrue(asignaciones.Any(a => a.Usuario.Id == _usuario1.Id && a.Rol == Rol.Administrador));
            Assert.IsTrue(asignaciones.Any(a => a.Usuario.Id == _usuario2.Id && a.Rol == Rol.Miembro));
            Assert.IsTrue(asignaciones.Any(a => a.Usuario.Id == _usuario1.Id && a.Rol == Rol.Lider));
        }

        [TestMethod]
        public void UsuarioEsLiderDeProyecto_DevuelveTrueSiEsLider()
        {
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario1, Rol.Lider));
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto2, _usuario2, Rol.Lider));

            Assert.IsTrue(_service.UsuarioEsLiderDeProyecto(_usuario1.Id, _proyecto1.Id));
            Assert.IsFalse(_service.UsuarioEsLiderDeProyecto(_usuario2.Id, _proyecto1.Id));
            Assert.IsFalse(_service.UsuarioEsLiderDeProyecto(_usuario1.Id, _proyecto2.Id));
        }

        [TestMethod]
        public void UsuarioEsLiderDeProyecto_DevuelveFalseSiNoEsLider()
        {
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario1, Rol.Administrador));
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario2, Rol.Lider));
            _repoAsignaciones.Add(new AsignacionProyecto(_proyecto1, _usuario2, Rol.Miembro));

            Assert.IsFalse(_service.UsuarioEsLiderDeProyecto(_usuario1.Id, _proyecto1.Id));
        }

        [TestMethod]
        public void GetLiderDeProyecto_DevuelveElLiderCorrectamente()
        {
            AsignacionProyecto asignLider = new AsignacionProyecto(_proyecto1, _usuario2, Rol.Lider);
            _repoAsignaciones.Add(asignLider);

            UsuarioDTO liderDTO = _service.GetLiderDeProyecto(_proyecto1.Id);

            Assert.IsNotNull(liderDTO);
            Assert.AreEqual(_usuario2.Id, liderDTO.Id);
            Assert.AreEqual(_usuario2.Nombre, liderDTO.Nombre);
            Assert.AreEqual(_usuario2.Apellido, liderDTO.Apellido);
            Assert.AreEqual(_usuario2.Email, liderDTO.Email);
        }
    }
}
