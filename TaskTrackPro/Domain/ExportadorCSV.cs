using System.Text;

namespace Domain;

public class ExportadorCSV : Exportador
{
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