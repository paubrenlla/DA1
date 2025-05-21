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
        throw new NotImplementedException();
    }

    public Usuario? BuscarUsuarioPorCorreo(Usuario usuario)
    {
        throw new NotImplementedException();
    }
}