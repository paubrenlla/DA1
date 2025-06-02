using DTOs;

namespace Services;

public interface IUsuarioService
{
    UsuarioDTO GetById(int id);
    void CrearUsuario(UsuarioDTO dto);
    void Delete(UsuarioDTO dto);
    UsuarioDTO GetByEmail(string email);
    UsuarioDTO BuscarUsuarioPorCorreoYContraseña(string email, string contraseña);
    void ConvertirEnAdmin(UsuarioDTO usuario);
    bool EsAdmin(UsuarioDTO usuario);
}