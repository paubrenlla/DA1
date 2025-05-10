using System.Reflection;
using BusinessLogic;

namespace BusinessLogic_Tests;

[TestClass]
public class UsuarioTests
{
    [TestInitialize]
    public void Setup()
    {
        typeof(Usuario)
            .GetField("_contadorId", BindingFlags.Static | BindingFlags.NonPublic)
            ?.SetValue(null, 1);
    }
    
    [TestMethod]
    public void ConstructorConDatosCorrectos()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));

        Assert.AreEqual(u.Email, "example@email.com");
        Assert.AreEqual(u.Nombre, "Nombre");
        Assert.AreEqual(u.Apellido, "Apellido");
        Assert.AreEqual(u.Pwd, "RXNWYWxpZGExIQ==");
        Assert.AreEqual(u.FechaNacimiento, new DateTime(2000, 01, 01));
    }
    
    [TestMethod]
    public void UsuariosConIdCorrecta()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        Usuario u2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));

        Assert.AreEqual(u.Id, 1);
        Assert.AreEqual(u2.Id, 2);
    }
    
    [TestMethod]
    public void UsuariosConIdCorrectaTrasCatchearExcpecion()
    {
        try
        {
            Usuario u = new Usuario("", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        }
        catch(Exception ex){}
        
        Usuario u2 = new Usuario("example2@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));

        Assert.AreEqual(u2.Id, 1);
    }

    [TestMethod]
    public void ModificarAtributosDeUnUsuarioExistente()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));
        string nuevoMail="example@email.com";
        string nuevoNombre="Pepe";
        string nuevoApellido="Rodriguez";
        string nuevoPwd="EsValida1!";
        DateTime nuevoFechaNacimiento = new DateTime(2000, 01, 01);
        u.Modificar(nuevoMail,nuevoNombre,nuevoApellido,nuevoPwd,nuevoFechaNacimiento);
        
        Assert.AreEqual(nuevoMail, u.Email);
        Assert.AreEqual(nuevoNombre, u.Nombre);
        Assert.AreEqual(nuevoApellido, u.Apellido);
        Assert.AreEqual(Usuario.EncriptarPassword(nuevoPwd), u.Pwd);
        Assert.AreEqual(nuevoFechaNacimiento, u.FechaNacimiento);
    }
    

    [TestMethod]
    public void ConstructorUsuarioEmailNulo_LanzaArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
            new Usuario(null, "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1)));
    }

    [TestMethod]
    public void ConstructorUsuarioEmailVacio_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
            new Usuario("", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1)));
    }

    [TestMethod]
    public void ConstructorUsuarioNombreVacio_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
            new Usuario("example@email.com", "", "Apellido", "EsValida1!", new DateTime(2000, 1, 1)));
    }

    [TestMethod]
    public void ConstructorUsuarioNombreNulo_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
            new Usuario("example@email.com", null, "Apellido", "EsValida1!", new DateTime(2000, 1, 1)));
    }

    [TestMethod]
    public void ConstructorUsuarioApellidoVacio_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
            new Usuario("example@email.com", "Nombre", "", "EsValida1!", new DateTime(2000, 1, 1)));
    }

    [TestMethod]
    public void ConstructorUsuarioApellidoNulo_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
            new Usuario("example@email.com", "Nombre", null, "EsValida1!", new DateTime(2000, 1, 1)));
    }

    [TestMethod]
    public void ConstructorUsuarioContraseñaVacia_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
            new Usuario("example@email.com", "Nombre", "Apellido", "", new DateTime(2000, 1, 1)));
    }

    [TestMethod]
    public void ConstructorUsuarioContraseñaNula_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
            new Usuario("example@email.com", "Nombre", "Apellido", null, new DateTime(2000, 1, 1)));
    }

    [TestMethod]
    public void ConstructorUsuarioContraseñaConEspacios_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentNullException>(() =>
            new Usuario("example@email.com", "Nombre", "Apellido", "    ", new DateTime(2000, 1, 1)));
    }

    [TestMethod]
    public void ConstructorUsuarioFechaNacimientoFutura_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() =>
            new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", DateTime.Now.AddYears(1)));
    }

    [TestMethod]
    public void NombreSetValorValido_ActualizaCorrectamente()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        u.Nombre = "NuevoNombre";
        Assert.AreEqual("NuevoNombre", u.Nombre);
    }

    [TestMethod]
    public void NombreSetValorInvalido_LanzaExcepcion()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Assert.ThrowsException<ArgumentNullException>(() => u.Nombre = "");
        Assert.ThrowsException<ArgumentNullException>(() => u.Nombre = null);
    }

    [TestMethod]
    public void Apellido_SetValorNulo_LanzaArgumentNullException()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "Passw0rd!", new DateTime(2000, 1, 1));
        Assert.ThrowsException<ArgumentNullException>(() => u.Apellido = "");
        Assert.ThrowsException<ArgumentNullException>(() => u.Apellido = null);
    }

    [TestMethod]
    public void ApellidoSetValorValido_ActualizaCorrectamente()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "Passw0rd!", new DateTime(2000, 1, 1));
        u.Apellido = "NuevoApellido";
        Assert.AreEqual("NuevoApellido", u.Apellido);
    }

    [TestMethod]
    public void FechaNacimientoSetValorValido_ActualizaCorrectamente()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        u.FechaNacimiento = new DateTime(1999, 12, 31);
        Assert.AreEqual(new DateTime(1999, 12, 31), u.FechaNacimiento);
    }

    [TestMethod]
    public void FechaNacimientoSetValorFuturo_LanzaExcepcion()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Assert.ThrowsException<ArgumentException>(() => u.FechaNacimiento = DateTime.Now.AddYears(1));
    }

    [TestMethod]
    public void EmailSetValorValido_ActualizaCorrectamente()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        u.Email = "nuevo@email.com";
        Assert.AreEqual("nuevo@email.com", u.Email);
    }

    [TestMethod]
    public void EmailSetValorNulo_LanzaExcepcion()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Assert.ThrowsException<ArgumentNullException>(() => u.Email = "");
        Assert.ThrowsException<ArgumentNullException>(() => u.Email = null);
    }

    [TestMethod]
    public void PwdSetValorValido_ActualizaCorrectamente()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        u.Pwd = "NuevaEsValida1!";
        Assert.AreEqual("TnVldmFFc1ZhbGlkYTEh", u.Pwd);
    }

    [TestMethod]
    public void PwdSetValorInvalido_LanzaExcepcion()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Assert.ThrowsException<ArgumentNullException>(() => u.Pwd = "");
        Assert.ThrowsException<ArgumentNullException>(() => u.Pwd = null);
    }

    [TestMethod]
    public void ValidarContraseñaValorNulo_LanzaArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => Usuario.ValidarContraseña(null));
    }

    [TestMethod]
    public void ValidarContraseñaValorVacia_LanzaArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => Usuario.ValidarContraseña(""));
    }

    [TestMethod]
    public void ValidarContraseñaValorSoloEspacios_LanzaArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => Usuario.ValidarContraseña("    "));
    }

    [TestMethod]
    public void validarContraseñaMenosDeOchoCaracteres_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => Usuario.ValidarContraseña("Corta1!"));
    }

    [TestMethod]
    public void validarContraseñaSinMinuscula_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => Usuario.ValidarContraseña("MAYUSCULAS1!"));
    }

    [TestMethod]
    public void validarContraseñaSinMayuscula_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => Usuario.ValidarContraseña("minusculas1!"));
    }

    [TestMethod]
    public void validarContraseñaSinNumero_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => Usuario.ValidarContraseña("SinNumero!"));
    }
    
    [TestMethod]
    public void validarContraseñaSinCaracterEspecial_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => Usuario.ValidarContraseña("SinCEspecial1"));
    }
    
    [TestMethod]
    public void ValidarContraseñaContrasenaValida()
    {
        Usuario.ValidarContraseña("EsValida1!"); 
    }
    
    [TestMethod]
    public void ValidarEmailEmailValido_NoLanzaExcepcion()
    {
        Usuario.ValidarEmail("correo@example.com");
        Usuario.ValidarEmail("usuario.email+alias@subdominio.dominio.com");
        Usuario.ValidarEmail("nombre.apellido@dominio.co");
        Usuario.ValidarEmail("email@dominio.io");
    }
    
    [TestMethod]
    public void ValidarEmailEmailNulo_LanzaArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => Usuario.ValidarEmail(null));
    }

    [TestMethod]
    public void ValidarEmailEmailVacio_LanzaArgumentNullException()
    {
        Assert.ThrowsException<ArgumentNullException>(() => Usuario.ValidarEmail(""));
    }
    
    [TestMethod]
    public void ValidarEmailEmailSinArroba_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => Usuario.ValidarEmail("correo.example.com"));
    }

    [TestMethod]
    public void ValidarEmailEmailSinDominio_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => Usuario.ValidarEmail("correo@"));
    }

    [TestMethod]
    public void ValidarEmailEmailSinTLD_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => Usuario.ValidarEmail("correo@dominio"));
    }

    [TestMethod]
    public void ValidarEmailEmailConEspacios_LanzaArgumentException()
    {
        Assert.ThrowsException<ArgumentException>(() => Usuario.ValidarEmail("correo @dominio.com"));
    }

    [TestMethod]
    public void ValidarEmailEmailConGuionValido_NoLanzaExcepcion()
    {
        Usuario.ValidarEmail("correo@mi-dominio.com"); 
        Usuario.ValidarEmail("correo@sub_dominio.com");
    }
    
    
    [TestMethod]

    public void ValidarMetodoEncriptacion()
    {
        string cadenaEncriptada = "RXNWYWxpZGExIQ==";
        string cadenaEncriptadaPorUsuario= Usuario.EncriptarPassword("EsValida1!");
        Assert.AreEqual(cadenaEncriptada, cadenaEncriptadaPorUsuario);
    }
    
    [TestMethod]
    public void UsuarioEsCreadoConPassEncriptada()
    {
        string passEcriptada = "RXNWYWxpZGExIQ==";
        string passNoEncriptada= "EsValida1!";

        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido",passNoEncriptada, new DateTime(2000, 1, 1));
        Assert.AreEqual(passEcriptada, u.Pwd);
    }
    
    //Tests para generar contraseña aleatoria
    [TestMethod] 
    public void GenerarContraseñaAleatoria_GeneraNuevaContraseña()
    {
        Usuario usuario = new Usuario("test@email.com", "Paula", "Apellido", "OldPassword123!", new DateTime(1990, 5, 10));

        usuario.GenerarContraseñaAleatoria();

        Assert.AreNotEqual("OldPassword123!", usuario.Pwd);
    }
    
    //Tests para restablecer contraseña
    [TestMethod]
    public void ResetearContraseña_ReseteaAPorDefault()
    {
        Usuario usuario = new Usuario("test@email.com", "Paula", "Apellido", "OldPassword123!", new DateTime(1990, 5, 10));

        usuario.ResetearContraseña();
        
        string defaultPwdEncriptada = Usuario.EncriptarPassword(Usuario.CONTRASEÑA_DEFAULT);
        
        Assert.AreEqual(defaultPwdEncriptada, usuario.Pwd);
    }
}