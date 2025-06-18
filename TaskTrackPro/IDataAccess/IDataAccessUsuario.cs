using Domain;

namespace IDataAcces;

public interface IDataAccessUsuario : IDataAccessGeneric<Usuario>
{
    public Usuario? BuscarUsuarioPorCorreo(string email);

    public Usuario? buscarUsuarioPorCorreoYContraseña(string email, string contraseña);

    public bool ExisteUsuarioConCorreo(string email);
    
    public void Update(Usuario usuario);
}