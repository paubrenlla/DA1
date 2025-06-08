using DTOs;
using Services;

namespace Controllers
{
    public class UsuarioController
    {
        private readonly IUsuarioService _service;

        public UsuarioController(IUsuarioService service)
        {
            _service = service;
        }

        public UsuarioDTO GetById(int id)
        {
            return _service.GetById(id);
        }
        
        public List<UsuarioDTO> GetAll()
        {
            return _service.GetAll();
        }

        public void EliminarUsuario(UsuarioDTO usuario)
        {
            _service.Delete(usuario);
        }

        public UsuarioDTO BuscarUsuarioPorCorreoYContraseña(string email, string contraseña)
        {
            return _service.BuscarUsuarioPorCorreoYContraseña(email, contraseña);
        }

        public UsuarioDTO GetByEmail(string email)
        {
            return _service.GetByEmail(email);
        }

        public void ConvertirEnAdmin(int usuarioId)
        {
            _service.ConvertirEnAdmin(usuarioId);
        }

        public bool EsAdmin(int usuarioDtoId) 
        {
            return _service.EsAdmin(usuarioDtoId);
        }

        public void CrearUsuario(UsuarioConContraseñaDTO dto)
        {
            _service.CrearUsuario(dto);
        }

        public void ModificarUsuario(UsuarioConContraseñaDTO dto)
        {
            _service.ModificarUsuario(dto);
        }

        public string ResetearContraseña(int usuarioId)
        {
            return _service.ResetearContraseña(usuarioId);
        }

        public string GenerarContraseñaAleatoria(int usuarioId)
        {
            return _service.GenerarContraseñaAleatoria(usuarioId);
        }

        public string DesencriptarContraseña(int usuarioId)
        {
            return _service.DesencriptarContraseña(usuarioId);
        }
        

    }
}