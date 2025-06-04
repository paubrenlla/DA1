using DTOs;
using Domain;
using IDataAcces;

namespace Services;

public class UsuarioService : IUsuarioService
{
    private readonly IDataAccessUsuario _usuarioRepo;
    private readonly IProyectoService _serviceProyecto;
    
    public UsuarioService(IDataAccessUsuario usuarioRepo)
    {
        _usuarioRepo = usuarioRepo;
    }


    public UsuarioDTO GetById(int id)
    {
        return Convertidor.AUsuarioDTO(_usuarioRepo.GetById(id));
    }

    public void CrearUsuario(UsuarioDTO dto)
    {
        Usuario nuevo = new Usuario(dto.Email, dto.Nombre, dto.Apellido,dto.Contraseña, dto.FechaNacimiento);
        _usuarioRepo.Add(nuevo);
    }

    public void Delete(UsuarioDTO dto)
    {
        Usuario usuarioAEliminar = _usuarioRepo.GetById(dto.Id);
        
        if (usuarioAEliminar == null)
            throw new NullReferenceException("El usuario no existe");
        if(usuarioAEliminar.EsAdminSistema)
            throw new ArgumentException("El usuario es administrador del sistema");
        if(_serviceProyecto.UsuarioEsAdminDeAlgunProyecto(usuarioAEliminar.Id))
            throw new ArgumentException("El usuario es administrador de un proyecto");
        
        
        _usuarioRepo.Remove(usuarioAEliminar);
        // TODO Eliminar asignaciones de tarea cuando haga tarea service
    }

    public UsuarioDTO GetByEmail(string email)
    {
        Usuario? usuario = _usuarioRepo.BuscarUsuarioPorCorreo(email);
        return Convertidor.AUsuarioDTO(usuario);
    }

    public UsuarioDTO BuscarUsuarioPorCorreoYContraseña(string email, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contraseña))
            throw new ArgumentException("Correo y contraseña son obligatorios");
        
        Usuario? usuario = _usuarioRepo.buscarUsuarioPorCorreoYContraseña(email, contraseña);
        if (usuario == null)
            throw new ArgumentException("Credenciales inválidas");
        
        return Convertidor.AUsuarioDTO(usuario);
    }

    public void ConvertirEnAdmin(UsuarioDTO usuario)
    {
        Usuario usuarioAdmin = _usuarioRepo.GetById(usuario.Id);
        if (usuarioAdmin.EsAdminSistema)
            throw new ArgumentException("El usuario ya es administrador del sistema");
        usuarioAdmin.EsAdminSistema = true;
    }
    
    public bool EsAdmin(UsuarioDTO usuario)
    {
        Usuario? usuarioBuscado = _usuarioRepo.GetById(usuario.Id);
        return usuarioBuscado.EsAdminSistema;
    }
}