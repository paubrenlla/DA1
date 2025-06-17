using Domain;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_Tests
{
    [TestClass]
    public class DataAccessUsuarioTest
    {
        private SqlContext _context;
        private UsuarioDataAccess _usuarioRepo;

        [TestInitialize]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<SqlContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new SqlContext(options);
            _usuarioRepo = new UsuarioDataAccess(_context);
        }

        [TestMethod]
        public void AgregarUsuarioComun()
        {
            Usuario user1 = new Usuario(
                email: "user1@example.com",
                nombre: "Nombre1",
                apellido: "Apellido1",
                pwd: EncriptadorContrasena.EncriptarPassword("Contraseña1!!"),
                fechaNacimiento: new DateTime(1990, 1, 1)
            );
            _usuarioRepo.Add(user1);

            Assert.AreEqual(1, _usuarioRepo.GetAll().Count);

            Usuario user2 = new Usuario(
                email: "user2@example.com",
                nombre: "Nombre2",
                apellido: "Apellido2",
                pwd: EncriptadorContrasena.EncriptarPassword("Contraseña1!!"),
                fechaNacimiento: new DateTime(1991, 2, 2)
            );
            _usuarioRepo.Add(user2);

            List<Usuario> all = _usuarioRepo.GetAll();
            Assert.AreEqual(2, all.Count);
            Assert.AreEqual(user2.Email, all.Last().Email);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AgregarUsuarioQueYaExiste()
        {
            Usuario user = new Usuario(
                email: "dup@example.com",
                nombre: "Dup",
                apellido: "User",
                pwd: EncriptadorContrasena.EncriptarPassword("Contraseña1!!"),
                fechaNacimiento: new DateTime(1992, 3, 3)
            );
            _usuarioRepo.Add(user);

            Usuario duplicate = new Usuario(
                email: "dup@example.com",
                nombre: "Dup",
                apellido: "User",
                pwd: EncriptadorContrasena.EncriptarPassword("Contraseña1!!"),
                fechaNacimiento: new DateTime(1992, 3, 3)
            );
            _usuarioRepo.Add(duplicate);
        }

        [TestMethod]
        public void EliminarUsuario()
        {
            Usuario user = new Usuario(
                email: "toremove@example.com",
                nombre: "To",
                apellido: "Remove",
                pwd: EncriptadorContrasena.EncriptarPassword("Contraseña1!!"),
                fechaNacimiento: new DateTime(1993, 4, 4)
            );
            _usuarioRepo.Add(user);
            Assert.AreEqual(1, _usuarioRepo.GetAll().Count);

            _usuarioRepo.Remove(user);
            Assert.AreEqual(0, _usuarioRepo.GetAll().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EliminarUsuarioAdminLanzaError()
        {
            Usuario user = new Usuario(
                email: "admin@example.com",
                nombre: "Admin",
                apellido: "User",
                pwd: EncriptadorContrasena.EncriptarPassword("Contraseña1!!"),
                fechaNacimiento: new DateTime(1980, 5, 5)
            )
            {
                EsAdminSistema = true
            };
            _usuarioRepo.Add(user);

            _usuarioRepo.Remove(user);
        }

        [TestMethod]
        public void GetByIdDevuelveUsuarioCorrecto()
        {
            Usuario u1 = new Usuario("a@a.com", "Ana", "Alvarez", EncriptadorContrasena.EncriptarPassword("Contraseña1!!"), new DateTime(2000, 1, 1));
            Usuario u2 = new Usuario("b@b.com", "Beto", "Barrios", EncriptadorContrasena.EncriptarPassword("Contraseña1!!"), new DateTime(1999, 2, 2));
            _usuarioRepo.Add(u1);
            _usuarioRepo.Add(u2);

            Usuario result = _usuarioRepo.GetById(u2.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(u2.Email, result.Email);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void GetByIdNoExistenteLanzaError()
        {
            _usuarioRepo.GetById(999);
        }

        [TestMethod]
        public void BuscarUsuarioPorCorreoYContraseñaDevuelveUsuario()
        {
            Usuario u1 = new Usuario("a@a.com", "Ana", "Alvarez", EncriptadorContrasena.EncriptarPassword("Contraseña1!!"), new DateTime(2000, 1, 1));
            Usuario u2 = new Usuario("b@b.com", "Beto", "Barrios", EncriptadorContrasena.EncriptarPassword("Contraseña1!!"), new DateTime(1999, 2, 2));
            _usuarioRepo.Add(u1);
            _usuarioRepo.Add(u2);

            Usuario result = _usuarioRepo.buscarUsuarioPorCorreoYContraseña(u2.Email, u2.Pwd);

            Assert.IsNotNull(result);
            Assert.AreEqual(u2.Email, result.Email);
        }

        [TestMethod]
        public void BuscarUsuarioPorCorreoDevuelveUsuario()
        {
            Usuario u1 = new Usuario("a@a.com", "Ana", "Alvarez", EncriptadorContrasena.EncriptarPassword("Contraseña1!!"), new DateTime(2000, 1, 1));
            Usuario u2 = new Usuario("b@b.com", "Beto", "Barrios", EncriptadorContrasena.EncriptarPassword("Contraseña1!!"), new DateTime(1999, 2, 2));
            _usuarioRepo.Add(u1);
            _usuarioRepo.Add(u2);

            Usuario result = _usuarioRepo.BuscarUsuarioPorCorreo(u2.Email);

            Assert.IsNotNull(result);
            Assert.AreEqual(u2.Email, result.Email);
        }
        
        [TestMethod]
        public void ModificarUsuarioActualizaCorrectamente()
        {
            Usuario user = new Usuario("email@x.com", "Nombre", "Apellido", "Contraseña1!", new DateTime(1990, 1, 1));
            _usuarioRepo.Add(user);

            user.Modificar("nuevo@x.com", "NuevoNombre", "NuevoApellido", "Contraseña1!", new DateTime(1991, 2, 2));
            _usuarioRepo.Update(user);

            Usuario modificado = _usuarioRepo.GetById(user.Id);
            Assert.AreEqual("nuevo@x.com", modificado.Email);
        }
    }
}
