namespace Domain;

public static class ExportadorFactory
{
    public static Exportador Crear(string tipo)
    {
        return tipo.ToLower() switch
        {
            "csv" => new ExportadorCSV(),
            "json" => new ExportadorJSON(),
            _ => throw new ArgumentException("Tipo de exportador no válido: " + tipo)
        };
    }
}