using Domain;
using Domain.Observers;
using DTOs;
using IDataAcces;
using Moq;
using Services;
using IServices;

namespace Services_Tests
{
    [TestClass]
    public class UsuarioServiceTests
    {
        private UsuarioService _service;
        private Mock<IDataAccessUsuario> _mockUsuarioRepo;
        private Mock<IProyectoService> _mockProyectoService;
        private List<IUsuarioObserver> _observers;
        private Usuario _usuario1;
        private Usuario _usuario2;
        private UsuarioDTO _dto1;
        private UsuarioConContraseñaDTO _dtoConPwd;

        [TestInitialize]
        public void SetUp()
        {
            _mockUsuarioRepo = new Mock<IDataAccessUsuario>();
            _mockProyectoService = new Mock<IProyectoService>();
            _observers = new List<IUsuarioObserver>();
            _service = new UsuarioService(
                _mockUsuarioRepo.Object, _mockProyectoService.Object, _observers);

            _usuario1 = new Usuario(
                "u1@test.com",
                "Nombre1",
                "Apellido1",
                "Contraseña1!",
                new DateTime(1990, 1, 1));
            _usuario2 = new Usuario(
                "u2@test.com",
                "Nombre2",
                "Apellido2",
                "Contraseña1!",
                new DateTime(1991, 1, 1));
            _usuario2.EsAdminSistema = true;

            _dto1 = new UsuarioDTO
            {
                Id = 1,
                Nombre = "Nombre1",
                Apellido = "Apellido1",
                Email = "u1@test.com",
                FechaNacimiento = new DateTime(1990, 1, 1),
            };

            _dto1 = new UsuarioDTO
            {
                Id = 2,
                Nombre = "Nombre2",
                Apellido = "Apellido2",
                Email = "u2@test.com",
                FechaNacimiento = new DateTime(1991, 1, 1),
            };

            _dtoConPwd = new UsuarioConContraseñaDTO
            {
                Id               = 3,
                Email            = "nuevo@test.com",
                Nombre           = "Nuevo",
                Apellido         = "Usuario",
                FechaNacimiento  = new DateTime(2000, 1, 1),
                Contraseña       = "PwdSegura1!"
            };
        }

        [TestMethod]
        public void GetByIdDevuelveDTOBien()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);

            UsuarioDTO resultado = _service.GetById(1);

            Assert.AreEqual(_usuario1.Id, resultado.Id);
            Assert.AreEqual(_usuario1.Nombre, resultado.Nombre);
            Assert.AreEqual(_usuario1.Email, resultado.Email);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [TestMethod]
        public void CrearUsuarioGuardaUsuarioCorrectamente()
        {
            _mockUsuarioRepo.Setup(r => r.BuscarUsuarioPorCorreo(_dtoConPwd.Email)).Returns((Usuario)null);
            _mockUsuarioRepo.Setup(r => r.Add(It.IsAny<Usuario>()));

            _service.CrearUsuario(_dtoConPwd);

            _mockUsuarioRepo.Verify(r => r.Add(It.Is<Usuario>(u =>
                u.Nombre == _dtoConPwd.Nombre &&
                u.Email == _dtoConPwd.Email)), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void DeleteLanzaExcepcionCuandoEsAdminSistema()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(2)).Returns(_usuario2);

            _service.Delete(_dto1);
        }

        [TestMethod]
        public void GetByEmailDevuelveUsuarioCorrecto()
        {
            string email = "u1@test.com";
            _mockUsuarioRepo.Setup(r => r.BuscarUsuarioPorCorreo(email)).Returns(_usuario1);

            UsuarioDTO resultado = _service.GetByEmail(email);

            Assert.AreEqual(_usuario1.Id, resultado.Id);
            Assert.AreEqual(_usuario1.Email, resultado.Email);
            _mockUsuarioRepo.Verify(r => r.BuscarUsuarioPorCorreo(email), Times.Once);
        }

        [TestMethod]
        public void BuscarUsuarioPorCorreoYContraseñaDevuelveUsuarioCuandoCredencialesSonCorrectas()
        {
            string email = "u1@test.com";
            string contraseña = "Contraseña1!";
            string encriptada = EncriptadorContrasena.EncriptarPassword(contraseña);

            _mockUsuarioRepo
                .Setup(r => r.buscarUsuarioPorCorreoYContraseña(email, encriptada))
                .Returns(_usuario1);

            UsuarioDTO resultado = _service.BuscarUsuarioPorCorreoYContraseña(email, contraseña);

            Assert.AreEqual(_usuario1.Id, resultado.Id);
            _mockUsuarioRepo.Verify(r => r.buscarUsuarioPorCorreoYContraseña(email, encriptada), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuscarUsuarioPorCorreoYContraseñaLanzaExcepcionCuandoCredencialesSonVacias()
        {
            _service.BuscarUsuarioPorCorreoYContraseña(string.Empty, string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void BuscarUsuarioPorCorreoYContraseñaLanzaExcepcionCuandoCredencialesSonInvalidas()
        {
            string email = "u1@test.com";
            string contraseña = "incorrecta";
            string encriptada = EncriptadorContrasena.EncriptarPassword(contraseña);

            _mockUsuarioRepo
                .Setup(r => r.buscarUsuarioPorCorreoYContraseña(email, encriptada))
                .Returns((Usuario)null);

            _service.BuscarUsuarioPorCorreoYContraseña(email, contraseña);
        }

        [TestMethod]
        public void ConvertirEnAdminConvierteUsuarioCorrectamente()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);

            _service.ConvertirEnAdmin(1);

            Assert.IsTrue(_usuario1.EsAdminSistema);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConvertirEnAdminLanzaExcepcionCuandoYaEsAdmin()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(2)).Returns(_usuario2);

            _service.ConvertirEnAdmin(2);
        }


        [TestMethod]
        public void EsAdminDevuelveTrueCuandoUsuarioEsAdmin()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(2)).Returns(_usuario2);

            bool resultado = _service.EsAdmin(2);

            Assert.IsTrue(resultado);
            _mockUsuarioRepo.Verify(r => r.GetById(2), Times.Once);
        }

        [TestMethod]
        public void EsAdminDevuelveFalseCuandoUsuarioNoEsAdmin()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);

            bool resultado = _service.EsAdmin(1);

            Assert.IsFalse(resultado);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
        }


        [TestMethod]
        public void ModificarUsuarioCambiaDatosCorrectamente()
        {
            UsuarioConContraseñaDTO dtoModificado = new UsuarioConContraseñaDTO
            {
                Id               = _usuario1.Id,
                Email            = "nuevoEmail@test.com",
                Nombre           = "NombreNuevo",
                Apellido         = "ApellidoNuevo",
                FechaNacimiento  = new DateTime(1990, 5, 5),
                Contraseña       = "NuevaPwd1!"
            };

            _mockUsuarioRepo.Setup(r => r.GetById(_usuario1.Id)).Returns(_usuario1);
            _mockUsuarioRepo
                .Setup(r => r.BuscarUsuarioPorCorreo(dtoModificado.Email))
                .Returns((Usuario)null);

            _service.ModificarUsuario(dtoModificado);

            Assert.AreEqual(dtoModificado.Email, _usuario1.Email);
            Assert.AreEqual(dtoModificado.Nombre, _usuario1.Nombre);
            Assert.AreEqual(dtoModificado.Apellido, _usuario1.Apellido);
            Assert.AreEqual(dtoModificado.FechaNacimiento, _usuario1.FechaNacimiento);
            _mockUsuarioRepo.Verify(r => r.BuscarUsuarioPorCorreo(dtoModificado.Email), Times.Once);
            _mockUsuarioRepo.Verify(r => r.GetById(_usuario1.Id), Times.Once);
            _mockUsuarioRepo.Verify(r => r.Update(It.Is<Usuario>(u => u.Id == _usuario1.Id)), Times.Once);        }

        [TestMethod]
        public void ResetearContraseñaDevuelvePwdEncriptada()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);

            string encriptada = EncriptadorContrasena.EncriptarPassword(_service.ResetearContraseña(1));

            Assert.IsNotNull(encriptada);
            Assert.AreEqual(_usuario1.Pwd, encriptada);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [TestMethod]
        public void GenerarContraseñaAleatoriaDevuelveNuevaEncriptada()
        {
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);

            string anterior = _usuario1.Pwd;
            string nueva = _service.GenerarContraseñaAleatoria(1);

            Assert.IsNotNull(nueva);
            Assert.AreNotEqual(anterior, nueva);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
        }

        [TestMethod]
        public void DesencriptarContraseñaDevuelveTextoPlano()
        {
            string textoPlano = "Secreto1!";
            _usuario1.Pwd = textoPlano;
            _mockUsuarioRepo.Setup(r => r.GetById(1)).Returns(_usuario1);

            string resultado = _service.DesencriptarContraseña(1);

            Assert.AreEqual(textoPlano, resultado);
            _mockUsuarioRepo.Verify(r => r.GetById(1), Times.Once);
        }
        
        [TestMethod]
        public void GetAllDevuelveListaVaciaCuandoNoHayUsuarios()
        {
            _mockUsuarioRepo
                .Setup(r => r.GetAll())
                .Returns(new List<Usuario>());

            List<UsuarioDTO> resultado = _service.GetAll();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(0, resultado.Count);
            _mockUsuarioRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [TestMethod]
        public void GetAllDevuelveListaDeUsuarioDTOs()
        {
            List<Usuario> lista = new List<Usuario> { _usuario1, _usuario2 };

            _mockUsuarioRepo
                .Setup(r => r.GetAll())
                .Returns(lista);

            List<UsuarioDTO> resultado = _service.GetAll();

            Assert.IsNotNull(resultado);
            Assert.AreEqual(2, resultado.Count);

            Assert.AreEqual(_usuario1.Id, resultado[0].Id);
            Assert.AreEqual(_usuario1.Email, resultado[0].Email);
            Assert.AreEqual(_usuario1.Nombre, resultado[0].Nombre);
            Assert.AreEqual(_usuario1.Apellido, resultado[0].Apellido);

            Assert.AreEqual(_usuario2.Id, resultado[1].Id);
            Assert.AreEqual(_usuario2.Email, resultado[1].Email);
            Assert.AreEqual(_usuario2.Nombre, resultado[1].Nombre);
            Assert.AreEqual(_usuario2.Apellido, resultado[1].Apellido);

            _mockUsuarioRepo.Verify(r => r.GetAll(), Times.Once);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CrearUsuario_LanzaExcepcionCuandoEmailDuplicado()
        {
            UsuarioConContraseñaDTO dto = new UsuarioConContraseñaDTO {
                Email = "existe@test.com",
                Nombre = "X", Apellido = "Y",
                FechaNacimiento = DateTime.Today,
                Contraseña = "Pwd1!"
            };
            _mockUsuarioRepo
                .Setup(r => r.BuscarUsuarioPorCorreo(dto.Email))
                .Returns(new Usuario(dto.Email, "A", "B", dto.Contraseña, dto.FechaNacimiento));

            _service.CrearUsuario(dto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ModificarUsuario_LanzaExcepcionCuandoEmailDuplicado()
        {
            Usuario existing = new Usuario("dup@test.com", "A", "B", "Pwd!", DateTime.Today) { Id = 5 };
            UsuarioConContraseñaDTO dto = new UsuarioConContraseñaDTO {
                Id = 7,
                Email = existing.Email,
                Nombre = "Nuevo", Apellido = "User",
                FechaNacimiento = DateTime.Today,
                Contraseña = "Pwd2!"
            };

            _mockUsuarioRepo
                .Setup(r => r.BuscarUsuarioPorCorreo(dto.Email))
                .Returns(existing);

            _service.ModificarUsuario(dto);
        }


    }
}
