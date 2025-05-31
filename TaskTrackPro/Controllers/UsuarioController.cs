using Domain;
using DTOs;
using IDataAcces;
using Services;
namespace Controllers;


public class UsuarioController
{
    private readonly IUsuarioService _service;


    public UsuarioController(IUsuarioService s)
    {
        _service = s;
    }


    public UsuarioDTO BuscarUsuarioPorId(int id)
    {
        return _service.GetById(id);
    }
    
    public void AgregarUsuario(UsuarioDTO dto)
    {
        _service.CrearUsuario(dto);
    }


    public void EliminarUsuario(UsuarioDTO dto)
    {
        _service.Delete(dto);
    }

    public UsuarioDTO BuscarUsuarioPorCorreoYContraseña(string email, string contraseña)
    {
        return _service.BuscarUsuarioPorCorreoYContraseña(email, contraseña);
    }


    public UsuarioDTO BuscarUsuarioPorCorreo(string email)
    {
        return _service.GetByEmail(email);
    }

    public void ConvertirEnAdmin(UsuarioDTO usuario)
    {
        _service.ConvertirEnAdmin(usuario);
    }

    public bool EsAdmin(UsuarioDTO usuario)
    {
        return _service.EsAdmin(usuario);
    }
    
}