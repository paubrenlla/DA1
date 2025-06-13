using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Domain;

public class Usuario
{
    public const int MINIMO_LARGO_CONTRASEÑA = 8;
    public const string CONTRASEÑA_DEFAULT = "1Contraseña!";
    private static int _contadorId = 1;

    public Usuario()
    {
        // Necesario para la deserialización con System.Text.Json
        // el JSON se deserialica en un objeto Usuario sin errores
        // y luego las propiedades se asignan una por una usando los setters públicos
    }

    public int Id { get; set; }
    private string _email;
    private string _nombre;
    private string _apellido;
    private string _pwd;
    private DateTime _fechaNacimiento;
    public ICollection<Tarea> TareasAsignadas { get; set; } = new List<Tarea>();
    public ICollection<Notificacion> NotificacionesRecibidas { get; set; } = new List<Notificacion>();
    public ICollection<Notificacion> NotificacionesVistas { get; set; } = new List<Notificacion>();
    public bool EsAdminSistema { get; set; }

    public Usuario(string email, string nombre, string apellido, string pwd, DateTime fechaNacimiento)
    {
        Email = email;
        Nombre = nombre;
        Apellido = apellido;
        Pwd = pwd;
        FechaNacimiento = fechaNacimiento;
        EsAdminSistema = false;
    }

    public Usuario(int dtoId, string dtoEmail, string dtoNombre, string dtoApellido, string dtoContraseña, DateTime dtoFechaNacimiento)
    {
        Id = dtoId;
        Email = dtoEmail;
        Nombre = dtoNombre;
        Apellido = dtoApellido;
        FechaNacimiento = dtoFechaNacimiento;
        _pwd = dtoContraseña;
    }

    public string Email
    {
        get => _email;
        set
        {
            ValidarEmail(value);
            _email = value;
        }
    }

    public string Nombre
    {
        get => _nombre;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), "El nombre es requerido y no puede estar vacío.");
            _nombre = value;
        }
    }

    public string Apellido
    {
        get => _apellido;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException(nameof(value), "El apellido es requerido y no puede estar vacío.");
            _apellido = value;
        }
    }
    
    
    [JsonIgnore] //Para que al deserializar el JSON no use este setter, así no valida
                 // ni vuelve a encriptar una constraseña ya encriptada
    public string Pwd
    {
        get => _pwd;
        set
        {
            ValidarContraseña(value);
            _pwd = EncriptarPassword(value);
        }
    }
    
    // Agregamos este getter y setter solo para serialización
    [JsonPropertyName("Pwd")]
    public string PwdSerializado
    {
        get => _pwd;
        set => _pwd = value;
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
    

    public void GenerarContraseñaAleatoria()
    {
        Pwd = GeneradorContraseña.GenerarContraseña(MINIMO_LARGO_CONTRASEÑA);
        
        Notificacion notificacion = new Notificacion("Su contraseña fue regenerada.");
        notificacion.AgregarUsuario(this);
    }

    public void ResetearContraseña()
    {
        Pwd = CONTRASEÑA_DEFAULT;
        
        Notificacion notificacion = new Notificacion("Su contraseña fue restablecida.");
        notificacion.AgregarUsuario(this);
    }
    
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
    
    public static void ValidarEmail(string value)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value), "El email no puede ser nulo o vacío.");

        if (!Regex.IsMatch(value, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9._-]+\.[a-zA-Z]{2,}$"))
            throw new ArgumentException("El email no tiene un formato válido.");
    }
    
    public static string EncriptarPassword(string passwordTextoPlano)
    {
        var passwordBytes = System.Text.Encoding.UTF8.GetBytes(passwordTextoPlano);
        return System.Convert.ToBase64String(passwordBytes);
    }

    public void Modificar(string email, string nombre, string apellido, string contraseña, DateTime fechaNacimiento)
    {
        Email = email;
        Nombre = nombre;
        Apellido = apellido;
        Pwd = contraseña;
        FechaNacimiento = fechaNacimiento;

        Notificacion notificacion = new Notificacion("Sus datos han sido actualizados.");
        notificacion.AgregarUsuario(this);
    }
    
    public static string DesencriptarPassword(string contraseñaEncriptada)
    {
        byte[] contraseñaBytes = Convert.FromBase64String(contraseñaEncriptada);
        return System.Text.Encoding.UTF8.GetString(contraseñaBytes);
    }
    
    public void AgregarAdmin()
    {
        if (EsAdminSistema)
            throw new ArgumentException("El usuario ya es administrador");

        EsAdminSistema = true;
    }
}