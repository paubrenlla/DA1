using Domain;

namespace Domain_Tests;

using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class ExportadorCSVTests
{
    [TestMethod]
    public void ExportarCSV_ConRecursos_IncluyeRecursosEnCSV()
    {
        // Arrange
        var exportador = new ExportadorCSV();
        string titulo = "Leer artículo";
        string descripcion = "Leer artículo de investigación para clase";
        DateTime fechaInicio = DateTime.Today;
        TimeSpan duracion = TimeSpan.FromHours(2);
        bool esCritica = false;

        Tarea tarea1 = new Tarea(titulo, descripcion, fechaInicio, duracion, esCritica);
        Recurso recurso = new Recurso("Recurso Test", "tipo", "descripcion", true, 3);

        List<Proyecto> proyectos = new List<Proyecto>();
        Proyecto nuevo = new Proyecto("test", "descripcion", DateTime.Today);
        nuevo.agregarTarea(tarea1); // ¡Añade la tarea al proyecto!
        proyectos.Add(nuevo);

        List<AsignacionRecursoTarea> asignaciones = new List<AsignacionRecursoTarea>
        {
            new AsignacionRecursoTarea 
            { 
                Tarea = tarea1, 
                Recurso = recurso,
                CantidadNecesaria = 2 
            }
        };

        string rutaArchivo = exportador.Exportar(proyectos, asignaciones);
        string contenido = File.ReadAllText(rutaArchivo);

        StringAssert.Contains(contenido, "Recurso Test");
        StringAssert.Contains(contenido, "2");
        StringAssert.Contains(contenido, "Leer artículo");

        File.Delete(rutaArchivo);
    }
}