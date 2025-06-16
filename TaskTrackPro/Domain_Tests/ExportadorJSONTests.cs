using Domain;


namespace Domain_Tests
{
    [TestClass]
    public class ExportadorJSONTests
    {
        [TestMethod]
        public void ExportarJSON_DatosValidos_GeneraArchivoCorrecto()
        {
            var exportador = new ExportadorJSON();
            string nombre = "Proyecto A";
            string descripcion = "Este es un proyecto para test";
            DateTime fechaInicio = DateTime.Today;
            var proyectos = new List<Proyecto>
            {
                new Proyecto(nombre, descripcion, fechaInicio)
            };
            var asignaciones = new List<AsignacionRecursoTarea>();
    
    
            string rutaArchivo = exportador.Exportar(proyectos, asignaciones);
    
    
            Assert.IsNotNull(rutaArchivo);
            Assert.IsTrue(File.Exists(rutaArchivo));
            Assert.IsTrue(rutaArchivo.EndsWith(".json"));
            File.Delete(rutaArchivo);
        }
    
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExportarJSON_ProyectosNull_DebeFallar()
        {
    
            var exportador = new ExportadorJSON();
            List<Proyecto> proyectos = null;
            var asignaciones = new List<AsignacionRecursoTarea>();
    
    
            exportador.Exportar(proyectos, asignaciones);
        }
    
        [TestMethod]
        public void ExportarJSON_ConRecursos_IncluyeRecursosEnJSON()
        {
            var exportador = new ExportadorJSON();
            string titulo = "Leer artículo";
            string descripcion = "Leer artículo de investigación para clase";
            DateTime fechaInicio = DateTime.Today;
            TimeSpan duracion = TimeSpan.FromHours(2); 
            bool esCritica = false;

            Tarea tarea1 = new Tarea(titulo, descripcion, fechaInicio, duracion, esCritica);
            Recurso recurso = new Recurso("Recurso Test", "tipo", "descripcion", true, 3);
    
            var proyectos = new List<Proyecto>();
            Proyecto nuevo = new Proyecto("test", "descripcion", DateTime.Today);
            nuevo.agregarTarea(tarea1); 
            proyectos.Add(nuevo);

            var asignaciones = new List<AsignacionRecursoTarea>
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

            File.Delete(rutaArchivo);
        }
    }
}