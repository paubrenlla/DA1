using Domain;
using IDataAcces;
using System;

namespace DataAccess
{
    public class UsuarioDataAccess : IDataAccessUsuario
    {
        private readonly List<Usuario> _listaUsuarios;
        public UsuarioDataAccess()
        {
            _listaUsuarios = new List<Usuario>();
                Usuario admin = new Usuario(
                    "admin@prueba.com",
                    "Admin",
                    "Prueba",
                    "Admin123!",
                    new DateTime(1990, 1, 1)
                );
                admin.EsAdminSistema = true;
                _listaUsuarios.Add(admin);

                Usuario user = new Usuario(
                    "user@prueba.com",
                    "Usuario",
                    "Prueba",
                    "User1234!",
                    new DateTime(1995, 5, 5)
                );
                _listaUsuarios.Add(user);
        }

        public void Add(Usuario usuario)
        {
            if (_listaUsuarios.Contains(usuario) || ExisteUsuarioConCorreo(usuario.Email))
                throw new ArgumentException("Usuario ya existe");
            _listaUsuarios.Add(usuario);
        }

        public void Remove(Usuario usuario)
        {
            if (usuario.EsAdminSistema)
                throw new ArgumentException("El usuario es administrador");
            _listaUsuarios.Remove(usuario);
        }

        public Usuario? GetById(int id)
        {
            Usuario usuario = _listaUsuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
                throw new ArgumentException("No existe el usuario");
            return usuario;
        }

        public List<Usuario> GetAll()
        {
            return _listaUsuarios;
        }

        public Usuario? buscarUsuarioPorCorreoYContraseña(string email, string contraseña)
        {
            return _listaUsuarios.FirstOrDefault(u =>
                u.Email == email && u.Pwd == contraseña);
        }

        public Usuario? BuscarUsuarioPorCorreo(string email)
        {
            return _listaUsuarios.FirstOrDefault(u => u.Email == email);
        }

        public bool ExisteUsuarioConCorreo(string email)
        {
            return _listaUsuarios.Any(u => u.Email == email);
        }
    }
}
