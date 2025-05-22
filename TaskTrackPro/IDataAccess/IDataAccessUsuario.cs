using BusinessLogic;

namespace IDataAcces;

public interface IDataAccessUsuario : IDataAccessGeneric<Usuario>
{
    public Usuario? BuscarUsuarioPorCorreo(string email);

    public Usuario? buscarUsuarioPorCorreoYContraseña(string email, string contraseña);

    public bool ExisteUsuarioConCorreo(Usuario user);
}