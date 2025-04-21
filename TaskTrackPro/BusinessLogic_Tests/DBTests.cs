using BusinessLogic;

namespace BusinessLogic_Tests;

[TestClass]
public class DBTests
{
    [TestMethod]
    public void ConstructorSinParametros()
    {
        DB db = new DB();
        
        Assert.AreEqual(0, db.AdministradoresSistema.Count);
        Assert.AreEqual(0, db.ListaProyectos.Count);
        Assert.AreEqual(0, db.ListaRecursos.Count);
        Assert.AreEqual(0, db.ListaUsuarios.Count);
    }
    
    [TestMethod]
    public void ConstructorConUsarioAdmin()
    {
        Usuario user = new Usuario();
        DB db = new DB(user);
        
        Assert.AreEqual(1, db.AdministradoresSistema.Count);
        Assert.AreEqual(0, db.ListaProyectos.Count);
        Assert.AreEqual(0, db.ListaRecursos.Count);
        Assert.AreEqual(1, db.ListaUsuarios.Count);
        Assert.AreSame(user, db.AdministradoresSistema[0]);
        Assert.AreSame(user, db.ListaUsuarios[0]);
    }
    
    
    
}