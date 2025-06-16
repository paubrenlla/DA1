using System.Text.Json;

namespace Domain;

public class ExportadorJSON : Exportador
{
    public override string Exportar(List<Proyecto> ListaDeProyectos, List<AsignacionRecursoTarea> ListaDeAsignacionRecursos)
    {
        // Procesar y transformar los datos
        var proyectosFiltrados = ListaDeProyectos
            .OrderBy(p => p.FechaInicio)
            .Select(p => new
            {
                Proyecto = new
                {
                    Nombre = p.Nombre,
                    FechaInicio = p.FechaInicio.ToString("dd/MM/yyyy")
                },
                Tareas = p.TareasAsociadas
                    .OrderByDescending(t => t.Titulo)
                    .Select(t => new
                    {
                        Titulo = t.Titulo,
                        FechaInicio = t.FechaInicio.ToString("dd/MM/yyyy"),
                        Duracion = t.Duracion,
                        EsCritica = t.EsCritica ? "S" : "N",
                        Recursos = ListaDeAsignacionRecursos
                            .Where(a => a.Tarea.Id == t.Id)  
                            .Select(a => new
                            {
                                NombreRecurso = a.Recurso.Nombre,
                                TipoRecurso = a.Recurso.Tipo,
                                CantidadNecesaria = a.CantidadNecesaria
                            })
                            .ToList()
                    })
            }).ToList();

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        try
        {
            string jsonString = JsonSerializer.Serialize(proyectosFiltrados, options);
            string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;
            string nombreArchivo = "proyectos.json";
            string rutaArchivo = Path.Combine(directorioEjecucion, nombreArchivo);
            File.WriteAllText(rutaArchivo, jsonString);
        
            Console.WriteLine($"Archivo guardado en: {rutaArchivo}");
            return rutaArchivo;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error al serializar: {ex.Message}");
            return null; 
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error al guardar el archivo: {ex.Message}");
            return null;
        }
        
    }
}



