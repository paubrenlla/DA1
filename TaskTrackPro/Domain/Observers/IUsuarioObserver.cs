namespace Domain.Observers;

public interface IUsuarioObserver
{
    void CambioContraseña(Usuario usuario, string contraseña);
    
    void ConvertidoEnAdmin(Usuario usuario);
}