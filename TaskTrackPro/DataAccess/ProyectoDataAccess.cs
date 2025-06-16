using System.Text;
using System.Text.Json;
using Domain;
using IDataAcces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ProyectoDataAccess :IDataAccessProyecto
{
    private readonly SqlContext _context;
    
    public ProyectoDataAccess(SqlContext context)
    {
        _context = context;
    }
    public void Add(Proyecto proyecto)
    {
        if (_context.Proyectos.Any(p => p.Id == proyecto.Id))
            throw new ArgumentException("El Proyecto ya existe en el sistema.");

        _context.Proyectos.Add(proyecto);
        _context.SaveChanges();
    }
    
    public void Remove(Proyecto proyecto)
    {
       _context.Remove(proyecto);
       _context.SaveChanges();
    }
    
    public Proyecto GetById(int id)
    {
       Proyecto proyecto = _context.Proyectos
           .Include(p => p.TareasAsociadas)
           .ThenInclude(t=>t.TareasDependencia)
           .ThenInclude(t=>t.TareasSucesoras)
           .FirstOrDefault(p => p.Id == id);
       if (proyecto is null)
           throw new ArgumentException(nameof(id), "El proyecto no existe.");
       return proyecto;
    }

    public List<Proyecto> GetAll()
    {
        return _context.Proyectos.Include(p=> p.TareasAsociadas).ToList();
    }

    public void Update(Proyecto proyecto)
    {
        _context.Update(proyecto);
        _context.SaveChanges();
    }
    

    public string ExportarJSON()
    {
        // Procesar y transformar los datos
        var proyectosFiltrados = _context.Proyectos
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
            foreach (var proyecto in _context.Proyectos.Include(p=>p.TareasAsociadas).OrderBy(p => p.FechaInicio))
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