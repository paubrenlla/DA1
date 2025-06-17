using DTOs;
using Domain;
using Domain.Observers;
using IDataAcces;

namespace Services;

public class UsuarioService : IUsuarioService
{
    private readonly IDataAccessUsuario _usuarioRepo;
    private readonly IProyectoService _serviceProyecto;

    private readonly List<IUsuarioObserver> _observers;
    
    public UsuarioService(IDataAccessUsuario usuarioRepo, IProyectoService serviceProyecto, IEnumerable<IUsuarioObserver> initialObservers)
    {
        _usuarioRepo     = usuarioRepo;
        _serviceProyecto = serviceProyecto;
        _observers = new List<IUsuarioObserver>(initialObservers);
    }

    public UsuarioDTO GetById(int id)
    {
        return Convertidor.AUsuarioDTO(_usuarioRepo.GetById(id));
    }

    public List<UsuarioDTO> GetAll()
    {
        return _usuarioRepo.GetAll()
            .Select(u => Convertidor.AUsuarioDTO(u))
            .ToList();
    }

    public void CrearUsuario(UsuarioConContraseñaDTO dto)
    {
        if(ExisteUsuarioConCorreo(dto.Email))
            throw new ArgumentException("Usuario con ese correo ya existe");
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
        
        _serviceProyecto.EliminarAsignacionesDeUsuario(usuarioAEliminar.Id);
        _usuarioRepo.Remove(usuarioAEliminar);
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
        
        Usuario? usuario = _usuarioRepo.buscarUsuarioPorCorreoYContraseña(email, EncriptadorContrasena.EncriptarPassword(contraseña));
        if (usuario == null)
            throw new ArgumentException("Credenciales inválidas");
        
        return Convertidor.AUsuarioDTO(usuario);
    }

    public void ConvertirEnAdmin(int usuarioId)
    {
        Usuario usuarioAdmin = _usuarioRepo.GetById(usuarioId);
        if (usuarioAdmin.EsAdminSistema)
            throw new ArgumentException("El usuario ya es administrador del sistema");
        usuarioAdmin.EsAdminSistema = true;
        _usuarioRepo.Update(usuarioAdmin);
        foreach (IUsuarioObserver obs in _observers)
            obs.ConvertidoEnAdmin(usuarioAdmin);
    }

    
    public bool EsAdmin(int usuarioId)
    {
        Usuario? usuarioBuscado = _usuarioRepo.GetById(usuarioId);
        return usuarioBuscado.EsAdminSistema;
    }

    public void ModificarUsuario(UsuarioConContraseñaDTO dto)
    {
        Usuario user = _usuarioRepo.GetById(dto.Id);
        if(ExisteUsuarioConCorreo(dto.Email) && dto.Email != user.Email)
            throw new ArgumentException("Usuario con ese correo ya existe");
        if (EncriptadorContrasena.DesencriptarPassword(user.Pwd) != dto.Contraseña)
        {
            foreach (IUsuarioObserver obs in _observers)
                obs.CambioContraseña(user, dto.Contraseña);
        }
        user.Modificar(dto.Email, dto.Nombre, dto.Apellido, dto.Contraseña, dto.FechaNacimiento);
        _usuarioRepo.Update(user);
        
    }

    private bool ExisteUsuarioConCorreo(string dtoEmail)
    {
        return _usuarioRepo.BuscarUsuarioPorCorreo(dtoEmail) != null;
    }

    public string ResetearContraseña(int usuarioId)
    {
        Usuario user = _usuarioRepo.GetById(usuarioId);
        user.ResetearContraseña();
        return EncriptadorContrasena.DesencriptarPassword(user.Pwd);
    }

    public string GenerarContraseñaAleatoria(int usuarioId)
    {
        Usuario user = _usuarioRepo.GetById(usuarioId);
        user.GenerarContraseñaAleatoria();
        return EncriptadorContrasena.DesencriptarPassword(user.Pwd);
    }

    public string DesencriptarContraseña(int usuarioId)
    {
        Usuario user = _usuarioRepo.GetById(usuarioId);
        return EncriptadorContrasena.DesencriptarPassword(user.Pwd);
    }
    
}