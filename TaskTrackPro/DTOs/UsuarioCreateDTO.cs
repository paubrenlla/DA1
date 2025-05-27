namespace DTOs;

public class UsuarioCreateDTO
{
    public string Email { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Contraseña { get; set; }
}

//TODO Creo que para eliminar, por las dudas lo dejo ahí