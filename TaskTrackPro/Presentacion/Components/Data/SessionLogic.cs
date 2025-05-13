using Blazored.LocalStorage;
using BusinessLogic;

namespace UserInterface.Data;

public class SessionLogic
{
    private const string CURRENT_USER = "current_user";

    private readonly ILocalStorageService _localStorage;
    private readonly DB _db;

    public SessionLogic(ILocalStorageService localStorage, DB db)
    {
        _localStorage = localStorage;
        _db = db;
    }

    public async Task Login(string email, string contraseña)
    {

        Usuario? user = _db.buscarUsuarioPorCorreoYContraseña(email, contraseña);

        if (user is null)
        {
            throw new Exception("Credenciales inválidas");
        }

        await _localStorage.SetItemAsync(CURRENT_USER, user);
    }

    public async Task<bool> isUserActive()
    {
        Usuario? user = await _localStorage.GetItemAsync<Usuario>(CURRENT_USER);
        return user is not null;
    }

    public async Task LogOut()
    {
        await _localStorage.RemoveItemAsync(CURRENT_USER);
    }

    public async Task<Usuario?> GetCurrentUser()
    {
        return await _localStorage.GetItemAsync<Usuario>(CURRENT_USER);
    }
    
    public async Task<bool> EsAdminSistema()
    {
        var user = await GetCurrentUser();
        if (user == null) return false;
    
        return _db.AdministradoresSistema
            .Any(a => a.Id == user.Id);
    }
}