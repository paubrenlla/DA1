using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Domain;
using IDataAcces;

namespace DataAccess;

public class ProyectoDataAccess :IDataAccessProyecto
{
    private List<Proyecto> _listaProyectos;
    
    public ProyectoDataAccess()
    {
        _listaProyectos = new List<Proyecto>();
    }
    public void Add(Proyecto proyecto)
    {
        if (_listaProyectos.Contains(proyecto))
            throw new ArgumentException("El proyecto ya existe");
        _listaProyectos.Add(proyecto);
    }
    
    public void Remove(Proyecto proyecto)
    {
       _listaProyectos.Remove(proyecto);
    }
    
    public Proyecto GetById(int id)
    {
        Proyecto proyecto = _listaProyectos.FirstOrDefault(p => p.Id == id);
        if (proyecto == null)
            throw new ArgumentException("No existe el proyecto");
        return proyecto;
    }

    public List<Proyecto> GetAll()
    {
        return _listaProyectos;
    }
    

    public string ExportarJSON()
    {
        // Procesar y transformar los datos
        var proyectosFiltrados = _listaProyectos
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
                        EsCritica = t.EsCritica ? "S" : "N"
                    })
            }).ToList();

        // Configuración para serialización
        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        try
        {
            string jsonString = JsonSerializer.Serialize(proyectosFiltrados, options);
        
            // Obtener ruta de guardado (directorio de ejecución + nombre archivo)
            string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;
            string nombreArchivo = "proyectos.json";
            string rutaArchivo = Path.Combine(directorioEjecucion, nombreArchivo);
        
            // Guardar el archivo
            File.WriteAllText(rutaArchivo, jsonString);
        
            Console.WriteLine($"Archivo guardado en: {rutaArchivo}");
        
            // Retornar la ruta para uso externo
            return rutaArchivo;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error al serializar: {ex.Message}");
            return null; // O lanzar una excepción personalizada
        }
        catch (IOException ex)
        {
            Console.WriteLine($"Error al guardar el archivo: {ex.Message}");
            return null;
        }
    }
    
    
    public string ExportarCSV()
    {
        try
        {
            // Procesar y transformar los datos a CSV
            var csvContent = new StringBuilder();
        
            // Encabezados del CSV
            csvContent.AppendLine("Proyecto,FechaInicio,Tarea,FechaInicioTarea,Duracion,EsCritica");
        
            // Datos de cada proyecto y sus tareas
            foreach (var proyecto in _listaProyectos.OrderBy(p => p.FechaInicio))
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

            // Obtener ruta de guardado
            string directorioEjecucion = AppDomain.CurrentDomain.BaseDirectory;
            string nombreArchivo = "proyectos.csv";
            string rutaArchivo = Path.Combine(directorioEjecucion, nombreArchivo);
        
            // Guardar el archivo
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

// Método auxiliar para escapar valores CSV
    private string EscapeCsvValue(string value)
    {
        if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
        {
            return $"\"{value.Replace("\"", "\"\"")}\"";
        }
        return value;
    }
    
    
}