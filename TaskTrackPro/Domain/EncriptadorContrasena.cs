namespace Domain;

public class EncriptadorContrasena
{
    public static string EncriptarPassword(string passwordTextoPlano)
    {
        var passwordBytes = System.Text.Encoding.UTF8.GetBytes(passwordTextoPlano);
        return System.Convert.ToBase64String(passwordBytes);
    }
    
    public static string DesencriptarPassword(string contraseñaEncriptada)
    {
        byte[] contraseñaBytes = Convert.FromBase64String(contraseñaEncriptada);
        return System.Text.Encoding.UTF8.GetString(contraseñaBytes);
    }
}