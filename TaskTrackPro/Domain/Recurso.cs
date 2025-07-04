﻿namespace Domain;

public class Recurso
{
    private static int _contadorId = 1;
    public string Nombre { get; set; }
    public string Tipo { get; set; }
    public string Descripcion { get; set; }
    public bool SePuedeCompartir { get; set; }
    public int CantidadDelRecurso { get; set; }
    public int CantidadEnUso { get; set; }
    
    public int  Id { get; set; }

    public Recurso(string nombre, string tipo, string descripcion, bool sePuedeCompartir, int cantidadDelRecurso) 
    {
        if (string.IsNullOrEmpty(nombre)) 
            throw new ArgumentNullException("Se debe ingresar un nombre.");
        
        if (string.IsNullOrEmpty(tipo))
            throw new ArgumentNullException("Se debe ingresar un tipo.");
        
        if (cantidadDelRecurso <= 0)
            throw new ArgumentException("La cantidad del recurso debe ser mayor a 0.");

        Nombre = nombre;
        Tipo = tipo;
        Descripcion = descripcion;
        SePuedeCompartir = sePuedeCompartir;
        CantidadDelRecurso = cantidadDelRecurso;
        CantidadEnUso = 0;
    }
    

    public bool EstaDisponible(int cantidad)
    {
        bool cantidadNecesaria = cantidad + CantidadEnUso <= CantidadDelRecurso;
        if(SePuedeCompartir && cantidadNecesaria)
        {
            return true;
        }
        return cantidadNecesaria;
    }
    
    public void ConsumirRecurso(int cantidad)
    {
        if (!EstaDisponible(cantidad)) return;
        CantidadEnUso += cantidad;
    }
    
    public void LiberarRecurso(int cantidad)
    {
        CantidadEnUso -= cantidad;
    }

    public bool EstaEnUso()
    {
        return CantidadEnUso > 0;
    }

    public void Modificar(string nombre, string tipo, string descripcion, int cantidad, bool compartir)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentNullException(nameof(nombre), "El nombre no puede estar vacío.");
        }
        
        if (string.IsNullOrWhiteSpace(tipo))
        {
            throw new ArgumentNullException(nameof(tipo), "El tipo no puede estar vacío.");
        }

        if (string.IsNullOrWhiteSpace(descripcion))
        {
            throw new ArgumentNullException(nameof(descripcion), "La descripcion no puede estar vacío.");
        }

        if (cantidad <= 0)
        {
            throw new ArgumentException("La cantidad debe ser mayor que cero.");
        }

        if (cantidad < CantidadEnUso)
        {
            throw new ArgumentException("La nueva cantidad no puede ser menor que la cantidad en uso.");
        }
        
        Nombre = nombre;
        Tipo = tipo;
        Descripcion = descripcion;
        CantidadDelRecurso = cantidad;
        SePuedeCompartir = compartir;
    }
}