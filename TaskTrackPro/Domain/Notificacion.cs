﻿namespace Domain;

public class Notificacion
{
    private static int _contadorId = 1;
    private string _mensaje;
    private List<Usuario> _usuariosNotificados;
    private List<Usuario> _vistaPorUsuarios;

    public int Id { get; }

    public string Mensaje
    {
        get => _mensaje;
        set => _mensaje = value;
    }
    public List<Usuario> UsuariosNotificados
    {
        get => _usuariosNotificados;
        set => _usuariosNotificados = value;
    }
    
    public List<Usuario> VistaPorUsuarios
    {
        get => _vistaPorUsuarios;
        set => _vistaPorUsuarios=value;
    }

    public Notificacion()
    {
        
    }
    public Notificacion(string mensaje)
    {
        Mensaje = mensaje;
        UsuariosNotificados = new List<Usuario>();
        VistaPorUsuarios = new List<Usuario>();
    }

    public void AgregarUsuario(Usuario usuario)
    {
        if (usuario == null) return;
        UsuariosNotificados.Add(usuario);
    }

    public void MarcarComoVista(Usuario usuario)
    {
        VistaPorUsuarios.Add(usuario);
    }

    public void AgregarUsuarios(List<Usuario> usuarios)
    {
        if (usuarios == null) return;
        foreach (Usuario usuario in usuarios)
        {
            AgregarUsuario(usuario);
        }
    }
}