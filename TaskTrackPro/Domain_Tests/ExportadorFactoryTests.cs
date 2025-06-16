using Domain;

namespace Domain_Tests;

[TestClass]
public class ExportadorFactoryTests
{
    [TestMethod]
    public void CrearExportador_ConFormatoJSON_DevuelveInstanciaExportadorJSON()
    {
        Exportador exportador = ExportadorFactory.Crear("JSON");
        Assert.IsInstanceOfType(exportador, typeof(ExportadorJSON));
    }

    [TestMethod]
    public void CrearExportador_ConFormatoCSV_DevuelveInstanciaExportadorCSV()
    {
        Exportador exportador = ExportadorFactory.Crear("CSV");
        Assert.IsInstanceOfType(exportador, typeof(ExportadorCSV));
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearExportador_ConFormatoInvalido_LanzaExcepcion()
    {
        Exportador exportador = ExportadorFactory.Crear("PDF");
    }
    
}