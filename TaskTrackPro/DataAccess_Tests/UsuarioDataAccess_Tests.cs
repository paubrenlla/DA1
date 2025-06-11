using Domain;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

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
            var user1 = new Usuario(
                email: "user1@example.com",
                nombre: "Nombre1",
                apellido: "Apellido1",
                pwd: Usuario.EncriptarPassword("Contraseña1!!"),
                fechaNacimiento: new DateTime(1990, 1, 1)
            );
            _usuarioRepo.Add(user1);

            Assert.AreEqual(1, _usuarioRepo.GetAll().Count);

            var user2 = new Usuario(
                email: "user2@example.com",
                nombre: "Nombre2",
                apellido: "Apellido2",
                pwd: Usuario.EncriptarPassword("Contraseña1!!"),
                fechaNacimiento: new DateTime(1991, 2, 2)
            );
            _usuarioRepo.Add(user2);

            var all = _usuarioRepo.GetAll();
            Assert.AreEqual(2, all.Count);
            Assert.AreEqual(user2.Email, all.Last().Email);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AgregarUsuarioQueYaExiste()
        {
            var user = new Usuario(
                email: "dup@example.com",
                nombre: "Dup",
                apellido: "User",
                pwd: Usuario.EncriptarPassword("Contraseña1!!"),
                fechaNacimiento: new DateTime(1992, 3, 3)
            );
            _usuarioRepo.Add(user);

            var duplicate = new Usuario(
                email: "dup@example.com",
                nombre: "Dup",
                apellido: "User",
                pwd: Usuario.EncriptarPassword("Contraseña1!!"),
                fechaNacimiento: new DateTime(1992, 3, 3)
            );
            _usuarioRepo.Add(duplicate);
        }

        [TestMethod]
        public void EliminarUsuario()
        {
            var user = new Usuario(
                email: "toremove@example.com",
                nombre: "To",
                apellido: "Remove",
                pwd: Usuario.EncriptarPassword("Contraseña1!!"),
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
            var user = new Usuario(
                email: "admin@example.com",
                nombre: "Admin",
                apellido: "User",
                pwd: Usuario.EncriptarPassword("Contraseña1!!"),
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
            var u1 = new Usuario("a@a.com", "Ana", "Alvarez", Usuario.EncriptarPassword("Contraseña1!!"), new DateTime(2000, 1, 1));
            var u2 = new Usuario("b@b.com", "Beto", "Barrios", Usuario.EncriptarPassword("Contraseña1!!"), new DateTime(1999, 2, 2));
            _usuarioRepo.Add(u1);
            _usuarioRepo.Add(u2);

            var result = _usuarioRepo.GetById(u2.Id);

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
            var u1 = new Usuario("a@a.com", "Ana", "Alvarez", Usuario.EncriptarPassword("Contraseña1!!"), new DateTime(2000, 1, 1));
            var u2 = new Usuario("b@b.com", "Beto", "Barrios", Usuario.EncriptarPassword("Contraseña1!!"), new DateTime(1999, 2, 2));
            _usuarioRepo.Add(u1);
            _usuarioRepo.Add(u2);

            var result = _usuarioRepo.buscarUsuarioPorCorreoYContraseña(u2.Email, u2.Pwd);

            Assert.IsNotNull(result);
            Assert.AreEqual(u2.Email, result.Email);
        }

        [TestMethod]
        public void BuscarUsuarioPorCorreoDevuelveUsuario()
        {
            var u1 = new Usuario("a@a.com", "Ana", "Alvarez", Usuario.EncriptarPassword("Contraseña1!!"), new DateTime(2000, 1, 1));
            var u2 = new Usuario("b@b.com", "Beto", "Barrios", Usuario.EncriptarPassword("Contraseña1!!"), new DateTime(1999, 2, 2));
            _usuarioRepo.Add(u1);
            _usuarioRepo.Add(u2);

            var result = _usuarioRepo.BuscarUsuarioPorCorreo(u2.Email);

            Assert.IsNotNull(result);
            Assert.AreEqual(u2.Email, result.Email);
        }
    }
}
