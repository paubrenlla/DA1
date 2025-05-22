using BusinessLogic;
using IDataAcces;

namespace Repositorios;


public class UsuarioDataAccess : IDataAccessUsuario
{
    private List<Usuario> _listaUsuarios;
    
    public UsuarioDataAccess()
    {
        _listaUsuarios = new List<Usuario>();
    }

    public void Add(Usuario usuario)
    {
        if (_listaUsuarios.Contains(usuario) || ExisteUsuarioConCorreo(usuario))
            throw new ArgumentException("Usuario ya existe");
        _listaUsuarios.Add(usuario);
    }

    public void Remove(Usuario usuario)
    {
        if (usuario.EsAdminSistema)
            throw new ArgumentException("El usuario es administrador");
        _listaUsuarios.Remove(usuario);
    }

    public Usuario GetById(int id)
    {
        return _listaUsuarios.FirstOrDefault(u => u.Id == id);
    }

    public List<Usuario> GetAll()
    {
        return _listaUsuarios;
    }

    public Usuario? buscarUsuarioPorCorreoYContraseña(string email, string contraseña)
    {
        return _listaUsuarios.FirstOrDefault(u =>
            u.Email == email && u.Pwd == Usuario.EncriptarPassword(contraseña));
    }

    public Usuario? BuscarUsuarioPorCorreo(string email)
    {
        return _listaUsuarios.FirstOrDefault(u => u.Email == email);
    }
    
    public bool ExisteUsuarioConCorreo(Usuario user)
    {
        return _listaUsuarios.Any(u => u.Email == user.Email);
    }
    
}