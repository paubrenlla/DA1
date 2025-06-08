using System.Security.Cryptography;

namespace Domain;

public class GeneradorContraseña
{
    private const string MINUSCULAS = "abcdefghijklmnopqrstuvwxyz";
    private const string MAYUSCULAS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NUMEROS = "0123456789";
    private const string CARACTERES_ESPECIALES = "!@#$%^&*()-_=+[]{}|;:,.<>?";

    public static string GenerarContraseña(int largoContraseña)
    {
        if (largoContraseña < 8)
            throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");

        string allChars = MINUSCULAS + MAYUSCULAS + NUMEROS + CARACTERES_ESPECIALES;
        char[] contraseña = new char[largoContraseña];

        using (RandomNumberGenerator generadorNumeroRandom = RandomNumberGenerator.Create())
        {
            contraseña[0] = MINUSCULAS[GetRandomIndex(generadorNumeroRandom, MINUSCULAS.Length)];
            contraseña[1] = MAYUSCULAS[GetRandomIndex(generadorNumeroRandom, MAYUSCULAS.Length)];
            contraseña[2] = NUMEROS[GetRandomIndex(generadorNumeroRandom, NUMEROS.Length)];
            contraseña[3] = CARACTERES_ESPECIALES[GetRandomIndex(generadorNumeroRandom, CARACTERES_ESPECIALES.Length)];

            for (int i = 4; i < largoContraseña; i++)
            {
                contraseña[i] = allChars[GetRandomIndex(generadorNumeroRandom, allChars.Length)];
            }

            contraseña = contraseña.OrderBy(_ => GetRandomIndex(generadorNumeroRandom, contraseña.Length)).ToArray();
        }

        return new string(contraseña);
    }

    private static int GetRandomIndex(RandomNumberGenerator generadorNumeroRandom, int max)
    {
        byte[] randomByte = new byte[1];
        do
        {
            generadorNumeroRandom.GetBytes(randomByte);
        } while (randomByte[0] >= max * (256 / max));
        
        return randomByte[0] % max;
    }

}