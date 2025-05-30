using Domain;
using DTOs;
using IDataAcces;

namespace Controllers;

public class UsuarioController
{
    private IDataAccessUsuario _repoUsuarios;
    private IDataAccessProyecto _repoProyectos;

    public UsuarioController(IDataAccessUsuario u, IDataAccessProyecto p)
    {
        _repoUsuarios = u;
        _repoProyectos = p;
    }

    public UsuarioDTO BuscarUsuarioPorId(int id)
    {
       Usuario usuario = _repoUsuarios.GetById(id);
       return Convertidor.AUsuarioDTO(usuario);
    }
    
    public void AgregarUsuario(UsuarioDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Contraseña))
            throw new ArgumentException("El correo y la contraseña no pueden estar vacíos");

        if (_repoUsuarios.ExisteUsuarioConCorreo(dto.Email))
            throw new ArgumentException("El correo ya está en uso");

        Usuario nuevoUsuario = new Usuario(dto.Email, dto.Nombre, dto.Apellido, dto.Contraseña, dto.FechaNacimiento);
        _repoUsuarios.Add(nuevoUsuario);
    }


    public void EliminarUsuario(UsuarioDTO dto)
    {
        Usuario usuarioAEliminar = _repoUsuarios.GetById(dto.Id);
        
        if(usuarioAEliminar.EsAdminSistema)
            throw new ArgumentException("El usuario es administrador del sistema");
        // if(EsAdminDeAlgunProyecto(usuarioAEliminar))
        //    throw new ArgumentException("El usuario es administrador de un proyecto");
        //TODO Eliminar asignaciones de tarea cuando haga tarea service

        _repoUsuarios.Remove(usuarioAEliminar);
    }

    // private bool EsAdminDeAlgunProyecto(Usuario usuario)
    // {
    //     return _repoProyectos.EsAdminDeAlgunProyecto(usuario);
    // } TODO hacer cuando se implemente la capa service

    public UsuarioDTO BuscarUsuarioPorCorreoYContraseña(string email, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contraseña))
            throw new ArgumentException("Correo y contraseña son obligatorios");

        Usuario? usuario = _repoUsuarios.buscarUsuarioPorCorreoYContraseña(email, contraseña);

        if (usuario == null)
            throw new ArgumentException("Credenciales inválidas");

        return Convertidor.AUsuarioDTO(usuario);
    }


    public UsuarioDTO BuscarUsuarioPorCorreo(string email)
    {
        Usuario? usuario = _repoUsuarios.BuscarUsuarioPorCorreo(email);
        
        return Convertidor.AUsuarioDTO(usuario);
    }

    public void ConvertirEnAdmin(UsuarioDTO usuario)
    {
        Usuario usuarioAdmin = _repoUsuarios.GetById(usuario.Id);
        if (usuarioAdmin.EsAdminSistema)
            throw new ArgumentException("El usuario ya es administrador del sistema");
        usuarioAdmin.EsAdminSistema = true;
    }

    public bool EsAdmin(UsuarioDTO usuario)
    {
        Usuario? usuarioAdmin = _repoUsuarios.GetById(usuario.Id);
        return usuarioAdmin.EsAdminSistema;
    }
    

}