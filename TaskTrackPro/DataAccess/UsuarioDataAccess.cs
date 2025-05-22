using BusinessLogic;
using IDataAcces;

namespace Repositorios;


public class UsuarioDataAccess : IDataAccessUsuario
{
    private List<Usuario> _listaUsuarios;
    public void Add(Usuario usuario)
    {
        _listaUsuarios.Add(usuario);
    }

    public void Remove(Usuario usuario)
    {
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
        return _listaUsuarios.FirstOrDefault(u => u.Email == email);    }
    }