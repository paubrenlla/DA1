namespace DTOs;

public class UsuarioConContraseñaDTO
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Contraseña { get; set; }
}