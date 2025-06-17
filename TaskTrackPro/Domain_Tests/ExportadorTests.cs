using Domain;

namespace Domain_Tests;

[TestClass]
public class ExportadorTests
{
    private class ExportadorConcreto : Exportador
    {
        public override string Exportar(List<Proyecto> ListaDeProyectos, 
            List<AsignacionRecursoTarea> ListaDeAsignacionRecursos)
        {
            return "EXPORTADO";
        }
    }

    [TestMethod]
    public void Exportar_DeberiaRetornarStringNoVacio()
    {
        var exportador = new ExportadorConcreto();
        var proyectos = new List<Proyecto>();
        var asignaciones = new List<AsignacionRecursoTarea>();
        var resultado = exportador.Exportar(proyectos, asignaciones);
        Assert.IsFalse(string.IsNullOrEmpty(resultado));
    }
}