using BusinessLogic;

namespace BusinessLogic_Tests;

[TestClass]
public class UsuarioTests
{
    //Tests de Constructor
    [TestMethod]
    public void ConstructorConDatosCorrectos()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 01, 01));

        Assert.AreEqual(u.Email, "example@email.com");
        Assert.AreEqual(u.Nombre, "Nombre");
        Assert.AreEqual(u.Apellido, "Apellido");
        Assert.AreEqual(u.Pwd, "EsValida1!");
        Assert.AreEqual(u.FechaNacimiento, new DateTime(2000, 01, 01));
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

    //Get y Set Nombre
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

    //Get y Set Apellido
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

    //Get y Set Fecha Nacimiento
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

    //Get y Set Email
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

    //Get y Set Contraseña
    [TestMethod]
    public void PwdSetValorValido_ActualizaCorrectamente()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        u.Pwd = "NuevaEsValida1!";
        Assert.AreEqual("NuevaEsValida1!", u.Pwd);
    }

    [TestMethod]
    public void PwdSetValorInvalido_LanzaExcepcion()
    {
        Usuario u = new Usuario("example@email.com", "Nombre", "Apellido", "EsValida1!", new DateTime(2000, 1, 1));
        Assert.ThrowsException<ArgumentNullException>(() => u.Pwd = "");
        Assert.ThrowsException<ArgumentNullException>(() => u.Pwd = null);
    }

    //Validar contraseña
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
}