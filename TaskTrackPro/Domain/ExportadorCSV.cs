using System.Text;

namespace Domain;

// Clase que exporta en formato CSV
public class ExportadorCSV : Exportador
{
    public override string Exportar(List<Proyecto> ListaDeProyectos, List<AsignacionRecursoTarea> ListaDeAsignacionRecursos)
    {
        try
        {
            var csvContent = new StringBuilder();
        
            csvContent.AppendLine("Proyecto,FechaInicio,Tarea,FechaInicioTarea,Duracion,EsCritica");
        
            foreach (var proyecto in ListaDeProyectos)
            {
                string proyectoNombre = EscapeCsvValue(proyecto.Nombre);
                string proyectoFecha = proyecto.FechaInicio.ToString("dd/MM/yyyy");
            
                foreach (var tarea in proyecto.TareasAsociadas.OrderByDescending(t => t.Titulo))
                {
                    csvContent.AppendLine(
                        $"{proyectoNombre}," +
                        $"{proyectoFecha}," +
                        $"{EscapeCsvValue(tarea.Titulo)}," +
                        $"{tarea.FechaInicio.ToString("dd/MM/yyyy")}," +
                        $"{tarea.Duracion}," +
                        $"{(tarea.EsCritica ? "S" : "N")}"
                    );
                }
            }
            string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;
            string nombreArchivo = "proyectos.csv";
            string rutaArchivo = Path.Combine(directorioEjecucion, nombreArchivo);
        
            File.WriteAllText(rutaArchivo, csvContent.ToString());
        
            Console.WriteLine($"Archivo CSV guardado en: {rutaArchivo}");
            return rutaArchivo;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al generar CSV: {ex.Message}");
            return null;
        }
    }

    private string EscapeCsvValue(string value)
    {
        if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
        return value;
    }
    }