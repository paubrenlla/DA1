using Domain;
using Domain.Enums;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_Tests
{
    [TestClass]
    public class AsignacionProyectoDataAccess_Tests
    {
        private SqlContext _context;
        private AsignacionProyectoDataAccess repo;
        private Usuario user1;
        private Usuario user2;
        private Proyecto proyecto1;
        private Proyecto proyecto2;
        private AsignacionProyecto asign1;
        private AsignacionProyecto asign2;
        private AsignacionProyecto asign3;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new SqlContext(options);
            repo = new AsignacionProyectoDataAccess(_context);
            user1 = new Usuario("u1@example.com", "Nombre1", "Apellido1", "Pass123!", new DateTime(1990,1,1));
            user2 = new Usuario("u2@example.com", "Nombre2", "Apellido2", "Pass123!", new DateTime(1991,1,1));
            proyecto1 = new Proyecto("P1", "Desc1", DateTime.Now.Date);
            proyecto2 = new Proyecto("P2", "Desc2", DateTime.Now.Date);
            user1.Id = 1;
            user2.Id = 2;

            asign1 = new AsignacionProyecto(proyecto1, user1, Rol.Miembro);
            asign2 = new AsignacionProyecto(proyecto2, user1, Rol.Administrador);
            asign3 = new AsignacionProyecto(proyecto1, user2, Rol.Miembro);
        }

        [TestMethod]
        public void AñadirAsignacionProyectoCorrectamente()
        {
            repo.Add(asign1);
            List<AsignacionProyecto> asignaciones = repo.GetAll();
            Assert.AreEqual(1, asignaciones.Count);
            Assert.IsTrue(asignaciones[0].Id == asign1.Id);
        }

        [TestMethod]
        public void ElimiarAsigancionEliminaDeLaLista()
        {
            repo.Add(asign1);
            Assert.IsTrue(repo.GetAll()[0].Id == asign1.Id);
            repo.Remove(asign1);
            Assert.IsTrue(repo.GetAll().Count() == 0);
        }

        [TestMethod]
        public void BuscarAsignacionProyectoPorID()
        {
            repo.Add(asign1);
            AsignacionProyecto asignacion = repo.GetById(asign1.Id);
            Assert.IsNotNull(asignacion);
            Assert.AreEqual(asign1, asignacion);
        }

        [TestMethod]
        public void GetAllDevuelveTodasLasAsignaciones()
        {
            repo.Add(asign1);
            repo.Add(asign2);
            List<AsignacionProyecto> asignaciones = repo.GetAll();
            Assert.AreEqual(2, asignaciones.Count);
            Assert.IsTrue(asignaciones[0].Id == asign1.Id);
            Assert.IsTrue(asignaciones[1].Id == asign2.Id);
        }

        [TestMethod]
        public void AsignacionesDelUsuarioFiltraPorUsuarios()
        {
            repo.Add(asign1);
            repo.Add(asign2);
            repo.Add(asign3);
            List<AsignacionProyecto> asignacionesDelUsuario = repo.AsignacionesDelUsuario(user1.Id);
            Assert.AreEqual(2, asignacionesDelUsuario.Count);
            Assert.IsTrue(asignacionesDelUsuario.All(a => a.Usuario.Id == user1.Id));
        }

        [TestMethod]
        public void UsuariosDelProyectoDevuelveLosUsuariosAsignadosAlProyecto()
        {
            repo.Add(asign1);
            repo.Add(asign2);
            repo.Add(asign3);
            List<AsignacionProyecto> asignacionesDelProyecto = repo.UsuariosDelProyecto(proyecto1.Id);
            Assert.AreEqual(2, asignacionesDelProyecto.Count);
            Assert.IsTrue(asignacionesDelProyecto.All(a => a.Proyecto.Id == proyecto1.Id));
        }

        [TestMethod]
        public void UsuarioEsAsignadoAProyecto()
        {
            repo.Add(asign1);
            Assert.IsTrue(repo.UsuarioEsAsignadoAProyecto(user1.Id, proyecto1.Id));
            Assert.IsFalse(repo.UsuarioEsAsignadoAProyecto(user2.Id, proyecto2.Id));
        }

        [TestMethod]
        public void UsuarioEsAdminDelProyecto()
        {
            repo.Add(asign2);
            Assert.IsTrue(repo.UsuarioEsAdminDelProyecto(user1.Id, proyecto2.Id));
            repo.Add(asign1);
            Assert.IsFalse(repo.UsuarioEsAdminDelProyecto(user1.Id, proyecto1.Id));
        }
        
        [TestMethod]
        public void GetAdminProyectoDevuelveCorrectamenteElAdmin()
        {
            repo.Add(asign1);
            repo.Add(asign2);
            repo.Add(asign3);

            AsignacionProyecto admin = repo.GetAdminProyecto(proyecto2.Id);

            Assert.IsNotNull(admin);
            Assert.AreEqual(asign2, admin);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetByIdLanzaExcepcionSiProyectoNoExiste()
        {
            ProyectoDataAccess repo = new ProyectoDataAccess(_context);

            int idInexistente = 999;

            repo.GetById(idInexistente);
        }
        
        [TestMethod]
        public void GetMiembrosDeProyectoDevuelveUsuariosCorrectamente()
        {
            repo.Add(asign1); 
            repo.Add(asign2); 
            repo.Add(asign3); 

            List<Usuario>? miembrosProyecto1 = repo.GetMiembrosDeProyecto(proyecto1.Id);
            List<Usuario>? miembrosProyecto2 = repo.GetMiembrosDeProyecto(proyecto2.Id);

            Assert.AreEqual(2, miembrosProyecto1.Count);
            Assert.IsTrue(miembrosProyecto1.Contains(user1));
            Assert.IsTrue(miembrosProyecto1.Contains(user2));

            Assert.AreEqual(1, miembrosProyecto2.Count);
            Assert.IsTrue(miembrosProyecto2.Contains(user1));
        }
        
        [TestMethod]
        public void GetMiembrosDeProyectoDevuelveListaVaciaSiNoHayAsignaciones()
        {
            List<Usuario>? miembros = repo.GetMiembrosDeProyecto(proyecto1.Id);

            Assert.IsNotNull(miembros);
            Assert.AreEqual(0, miembros.Count);
        }

    }
}
