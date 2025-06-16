using System.Text;

namespace Domain;

// Clase que exporta en formato CSV
public class ExportadorCSV : Exportador
{
    // public override string Exportar(List<Proyecto> ListaDeProyectos, List<AsignacionRecursoTarea> ListaDeAsignacionRecursos)
    // {
    //     try
    //     {
    //         var csvContent = new StringBuilder();
    //     
    //         csvContent.AppendLine("Proyecto,FechaInicio,Tarea,FechaInicioTarea,Duracion,EsCritica");
    //     
    //         foreach (var proyecto in ListaDeProyectos)
    //         {
    //             string proyectoNombre = EscapeCsvValue(proyecto.Nombre);
    //             string proyectoFecha = proyecto.FechaInicio.ToString("dd/MM/yyyy");
    //         
    //             foreach (var tarea in proyecto.TareasAsociadas.OrderByDescending(t => t.Titulo))
    //             {
    //                 csvContent.AppendLine(
    //                     $"{proyectoNombre}," +
    //                     $"{proyectoFecha}," +
    //                     $"{EscapeCsvValue(tarea.Titulo)}," +
    //                     $"{tarea.FechaInicio.ToString("dd/MM/yyyy")}," +
    //                     $"{tarea.Duracion}," +
    //                     $"{(tarea.EsCritica ? "S" : "N")}"
    //                 );
    //             }
    //         }
    //         string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;
    //         string nombreArchivo = "proyectos.csv";
    //         string rutaArchivo = Path.Combine(directorioEjecucion, nombreArchivo);
    //     
    //         File.WriteAllText(rutaArchivo, csvContent.ToString());
    //     
    //         Console.WriteLine($"Archivo CSV guardado en: {rutaArchivo}");
    //         return rutaArchivo;
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"Error al generar CSV: {ex.Message}");
    //         return null;
    //     }
    // }
    //
    // private string EscapeCsvValue(string value)
    // {
    //     if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
    //     {
    //         return $"\"{value.Replace("\"", "\"\"")}\"";
    //     }
    //     return value;
    // }
    
    
    public override string Exportar(List<Proyecto> proyectos, List<AsignacionRecursoTarea> asignaciones)
{
    
    var csvContent = new StringBuilder();
    csvContent.AppendLine("Proyecto,FechaInicio,Tarea,FechaInicioTarea,Duracion,EsCritica,Recurso,TipoRecurso,Cantidad");

    foreach (var proyecto in proyectos.OrderBy(p => p.FechaInicio))
    {
        var proyectoNombre = proyecto.Nombre ?? "Sin nombre";
        var proyectoFecha = proyecto.FechaInicio.ToString("dd/MM/yyyy");

        if (proyecto.TareasAsociadas?.Any() != true)
        {
            csvContent.AppendLine($"\"{proyectoNombre}\",{proyectoFecha},,,,,");
            continue;
        }

        foreach (var tarea in proyecto.TareasAsociadas.Where(t => t != null).OrderByDescending(t => t.Titulo))
        {
            var tareaNombre = tarea.Titulo ?? "Sin título";
            var tareaFecha = tarea.FechaInicio.ToString("dd/MM/yyyy");
            var tareaDuracion = tarea.Duracion;
            var tareaCritica = tarea.EsCritica ? "S" : "N";

            var recursos = asignaciones?
                .Where(a => a?.Tarea != null && a.Tarea.Id == tarea.Id)
                .ToList() ?? new List<AsignacionRecursoTarea>();

            if (!recursos.Any())
            {
                csvContent.AppendLine($"\"{proyectoNombre}\",{proyectoFecha},\"{tareaNombre}\",{tareaFecha},{tareaDuracion},{tareaCritica},,,");
                continue;
            }

            foreach (var recurso in recursos)
            {
                var recursoNombre = recurso.Recurso?.Nombre ?? "Sin nombre";
                var recursoTipo = recurso.Recurso?.Tipo ?? "Sin tipo";
                var cantidad = recurso.CantidadNecesaria;

                csvContent.AppendLine($"\"{proyectoNombre}\",{proyectoFecha},\"{tareaNombre}\",{tareaFecha},{tareaDuracion},{tareaCritica},\"{recursoNombre}\",{recursoTipo},{cantidad}");
            }
        }
    }

    string directorio = AppDomain.CurrentDomain.BaseDirectory;
    string nombreArchivo = $"proyectos_{DateTime.Now:yyyyMMddHHmmss}.csv";
    string rutaArchivo = Path.Combine(directorio, nombreArchivo);

    File.WriteAllText(rutaArchivo, csvContent.ToString());
    
    return rutaArchivo;
}
    
    
    
    
    
    
    
    }