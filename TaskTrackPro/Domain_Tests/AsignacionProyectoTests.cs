using Domain;
using Domain.Enums;

namespace Domain_Tests;

[TestClass]
public class AsignacionProyectoTests
{
    [TestMethod]
    public void CrearAsignacionProyectoCorrectamente()
    {
        Usuario usuario = new Usuario("hola@gmail.com", "Pepe","Perez", "Contraseña1!", new DateTime (2000,12,12));
        Proyecto proyecto = new Proyecto("Proyecto Test", "Test", DateTime.Today);
        AsignacionProyecto asigancion = new AsignacionProyecto(proyecto, usuario, Rol.Miembro);
        
        Assert.IsNotNull(asigancion);
        Assert.AreSame(proyecto, asigancion.Proyecto);
        Assert.AreSame(usuario, asigancion.Usuario);
        Assert.AreEqual(Rol.Miembro, asigancion.Rol);
    }
}