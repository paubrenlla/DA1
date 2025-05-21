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
        throw new NotImplementedException();
    }

    public Usuario GetById(int id)
    {
        throw new NotImplementedException();
    }

    public List<Usuario> GetAll()
    {
        throw new NotImplementedException();
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