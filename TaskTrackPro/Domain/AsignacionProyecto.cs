using Domain.Enums;

namespace Domain;

public class AsignacionProyecto
{
    private static int _contadorId = 1;
    private Proyecto _proyecto;
    private Usuario _usuario;
    private Rol _rol;
    
    public int Id { get; }
    public Proyecto Proyecto { get => _proyecto; set => _proyecto = value; }
    public Usuario Usuario { get => _usuario; set => _usuario = value; }
    public Rol Rol { get => _rol; set => _rol = value; }

    public AsignacionProyecto(Proyecto proyecto, Usuario usuario, Rol rol)
    {
        Proyecto = proyecto;
        Usuario = usuario;
        Rol = rol;
        Id = _contadorId++;
    }
}