using BusinessLogic;
using Services.DTOs;

namespace Services;

public static class Convertidor
{
    public static UsuarioDTO AUsuarioDTO(Usuario usuario)
    {
        return new UsuarioDTO
        {
            Id = usuario.Id,
            Email = usuario.Email,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            FechaNacimiento = usuario.FechaNacimiento,
            Contraseña = usuario.Pwd
        };
    }

    public static Usuario AUsuario(UsuarioDTO dto)
    {
        return new Usuario(dto.Id, dto.Email, dto.Nombre, dto.Apellido, dto.Contraseña, dto.FechaNacimiento);
    }
    
    public static Usuario AUsuario(UsuarioCreateDTO dto)
    {
        return new Usuario(dto.Email, dto.Nombre, dto.Apellido, dto.Contraseña, dto.FechaNacimiento);
    }

}
