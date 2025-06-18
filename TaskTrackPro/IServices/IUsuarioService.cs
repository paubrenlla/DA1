using DTOs;

namespace IServices;

public interface IUsuarioService
{
    UsuarioDTO GetById(int id);
    List<UsuarioDTO> GetAll();
    void CrearUsuario(UsuarioConContraseñaDTO dto);
    void Delete(UsuarioDTO dto);
    UsuarioDTO GetByEmail(string email);
    UsuarioDTO BuscarUsuarioPorCorreoYContraseña(string email, string contraseña);
    void ConvertirEnAdmin(int usuarioId);
    bool EsAdmin(int usuarioId);
    void ModificarUsuario(UsuarioConContraseñaDTO dto); 
    string ResetearContraseña(int usuarioId);
    string GenerarContraseñaAleatoria(int usuarioId);
    string DesencriptarContraseña(int usuarioId);
}