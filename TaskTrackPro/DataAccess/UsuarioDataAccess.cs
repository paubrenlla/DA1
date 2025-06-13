using Domain;
using IDataAcces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class UsuarioDataAccess : IDataAccessUsuario
    {
        private readonly SqlContext _context;

        public UsuarioDataAccess(SqlContext context)
        {
            _context = context;
        }

        public void Add(Usuario usuario)
        {
            if (ExisteUsuarioConCorreo(usuario.Email))
                throw new ArgumentException("Usuario ya existe");

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
        }

        public void Remove(Usuario usuario)
        {
            if (usuario.EsAdminSistema)
                throw new ArgumentException("El usuario es administrador");

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
        }

        public Usuario? GetById(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
                throw new ArgumentException("No existe el usuario");
            return usuario;
        }

        public List<Usuario> GetAll()
        {
            return _context.Usuarios.AsNoTracking().ToList();
        }
        
        public void Update(Usuario user)
        {
            _context.Usuarios.Update(user);
            _context.SaveChanges();
        }

        public Usuario? buscarUsuarioPorCorreoYContraseña(string email, string contraseña)
        {
            return _context.Usuarios
                .FirstOrDefault(u => u.Email == email && u.Pwd == contraseña);
        }

        public Usuario? BuscarUsuarioPorCorreo(string email)
        {
            return _context.Usuarios
                .FirstOrDefault(u => u.Email == email);
        }

        public bool ExisteUsuarioConCorreo(string email)
        {
            return _context.Usuarios.Any(u => u.Email == email);
        }
    }
}