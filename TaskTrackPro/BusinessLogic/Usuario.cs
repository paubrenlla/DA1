using System.Text.RegularExpressions;

namespace BusinessLogic;

public class Usuario
{
    private string _email;
    private string _nombre;
    private string _apellido;
    private string _pwd;
    private DateTime _fechaNacimiento;

    public Usuario(string email, string nombre, string apellido, string pwd, DateTime fechaNacimiento)
    {
        Email = email;
        Nombre = nombre;
        Apellido = apellido;
        Pwd = pwd;
        FechaNacimiento = fechaNacimiento;
    }
    
    //Properties
    public string Email
    {
        get => _email;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("El email es requerido y no puede estar vacío.", nameof(value));
            _email = value;
        }
    }

    public string Nombre
    {
        get => _nombre;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("El nombre es requerido y no puede estar vacío.", nameof(value));
            _nombre = value;
        }
    }

    public string Apellido
    {
        get => _apellido;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("El apellido es requerido y no puede estar vacío.", nameof(value));
            _apellido = value;
        }
    }

    public string Pwd
    {
        get => _pwd;
        set
        {
            ValidarContraseña(value);
            _pwd = value;
        }
    }

    public DateTime FechaNacimiento
    {
        get => _fechaNacimiento;
        set
        {
            if (value > DateTime.Now)
                throw new ArgumentException("La fecha de nacimiento no puede ser en el futuro.");
            
            _fechaNacimiento = value;
        }
    }
    
    //Metodos
    public static void ValidarContraseña(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentNullException(nameof(value), "La pwd es requerida");
        if (value.Length < 8)
            throw new ArgumentException(nameof(value), "La pwd debe tener al menos 8 caracteres");
        if (!Regex.IsMatch(value, @"^(?=.*[a-z])"))
            throw new ArgumentException("La pwd debe tener al menos una letra minuscula");
        if (!Regex.IsMatch(value, @"^(?=.*[A-Z])"))
            throw new ArgumentException("La pwd debe tener al menos una letra mayúscula");
        if (!Regex.IsMatch(value, @"^(?=.*\d)"))
            throw new ArgumentException("La pwd debe tener al menos un número");
        if (!Regex.IsMatch(value, @"^(?=.*[\W_])"))
            throw new ArgumentException("La pwd debe tener al menos un caractér especial");
    }
}