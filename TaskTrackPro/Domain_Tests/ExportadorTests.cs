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
        List<Proyecto> proyectos = new List<Proyecto>();
        List<AsignacionRecursoTarea> asignaciones = new List<AsignacionRecursoTarea>();
        string resultado = exportador.Exportar(proyectos, asignaciones);
        Assert.IsFalse(string.IsNullOrEmpty(resultado));
    }
}