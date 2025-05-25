using BusinessLogic;
using IDataAcces;
using Services.DTOs;

namespace Services;

public class UsuarioService
{
    private IDataAccessUsuario listaUsuarios;
    private IDataAccessProyecto listaProyecto;

    public UsuarioService(IDataAccessUsuario u, IDataAccessProyecto p)
    {
        listaUsuarios = u;
        listaProyecto = p;
    }

    public UsuarioDTO BuscarUsuarioPorId(int id)
    {
       Usuario usuario = listaUsuarios.GetById(id);
       return Convertidor.AUsuarioDTO(usuario);
    }
    
    public void AgregarUsuario(UsuarioCreateDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Contraseña))
            throw new ArgumentException("El correo y la contraseña no pueden estar vacíos");

        if (listaUsuarios.ExisteUsuarioConCorreo(dto.Email))
            throw new ArgumentException("El correo ya está en uso");

        Usuario nuevoUsuario = Convertidor.AUsuario(dto);
        listaUsuarios.Add(nuevoUsuario);
    }


    public void EliminarUsuario(UsuarioDTO dto)
    {
        Usuario usuarioAEliminar = listaUsuarios.GetById(dto.Id);
        
        if(usuarioAEliminar.EsAdminSistema)
            throw new ArgumentException("El usuario es administrador del sistema");
        if(EsAdminDeAlgunProyecto(usuarioAEliminar))
            throw new ArgumentException("El usuario es administrador de un proyecto");
        //TODO Eliminar asignaciones de tarea cuando haga tarea service

        listaUsuarios.Remove(usuarioAEliminar);
    }

    private bool EsAdminDeAlgunProyecto(Usuario usuario)
    {
        return listaProyecto.esAdminDeAlgunProyecto(usuario);
    }

    public UsuarioDTO BuscarUsuarioPorCorreoYContraseña(string email, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contraseña))
            throw new ArgumentException("Correo y contraseña son obligatorios");

        Usuario? usuario = listaUsuarios.buscarUsuarioPorCorreoYContraseña(email, contraseña);

        if (usuario == null)
            throw new ArgumentException("Credenciales inválidas");

        return Convertidor.AUsuarioDTO(usuario);
    }


    public UsuarioDTO BuscarUsuarioPorCorreo(string email)
    {
        Usuario? usuario = listaUsuarios.BuscarUsuarioPorCorreo(email);
        
        return Convertidor.AUsuarioDTO(usuario);
    }

    public void ConvertirEnAdmin(UsuarioDTO usuario)
    {
        Usuario usuarioAdmin = listaUsuarios.GetById(usuario.Id);
        if (usuarioAdmin.EsAdminSistema)
            throw new ArgumentException("El usuario ya es administrador del sistema");
        usuarioAdmin.EsAdminSistema = true;
    }

    public bool EsAdmin(UsuarioDTO usuario)
    {
        Usuario? usuarioAdmin = listaUsuarios.GetById(usuario.Id);
        return usuarioAdmin.EsAdminSistema;
    }
    
    //TODO hacer cuando tenga ProyectoService DTO

    // public bool UsuarioEsAdminDelProyecto(UsuarioDTO usuario, Proyecto p)
    // {
    //     Usuario usuarioAdmin = listaUsuarios.GetById(usuario.Id);
    //     listaProyecto.GetById()
    // }

}