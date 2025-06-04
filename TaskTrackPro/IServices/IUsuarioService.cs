using DTOs;

namespace Services;

public interface IUsuarioService
{
    UsuarioDTO GetById(int id);
    void CrearUsuario(UsuarioConContraseñaDTO dto);
    void Delete(UsuarioDTO dto);
    UsuarioDTO GetByEmail(string email);
    UsuarioDTO BuscarUsuarioPorCorreoYContraseña(string email, string contraseña);
    void ConvertirEnAdmin(UsuarioDTO usuario);
    bool EsAdmin(UsuarioDTO usuario);
    void ModificarUsuario(UsuarioConContraseñaDTO dto); 
    string ResetearContraseña(int usuarioId);
    string GenerarContraseñaAleatoria(int usuarioId);
    string DesencriptarContraseña(int usuarioId);
}