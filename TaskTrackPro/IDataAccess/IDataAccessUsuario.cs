using BusinessLogic;

namespace IDataAcces;

public interface IDataAccessUsuario : IDataAccessGeneric<Usuario>
{
    public Usuario? BuscarUsuarioPorCorreo(Usuario usuario);

    public Usuario? buscarUsuarioPorCorreoYContraseña(string email, string contraseña);
}