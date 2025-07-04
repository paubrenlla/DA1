﻿using Blazored.LocalStorage;
using DTOs;
using Controllers;

namespace UserInterface.Data
{
    public class SessionLogic
    {
        private const string CURRENT_USER = "current_user";

        private readonly ILocalStorageService _localStorage;
        private readonly UsuarioController _usuarioController;

        public SessionLogic(
            ILocalStorageService localStorage,
            UsuarioController usuarioController)
        {
            _localStorage = localStorage;
            _usuarioController = usuarioController;
        }

        public async Task Login(string email, string contraseña)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(contraseña))
            {
                throw new Exception("Credenciales inválidas");
            }

            UsuarioDTO userDto = _usuarioController.BuscarUsuarioPorCorreoYContraseña(email, contraseña);

            if (userDto == null)
            {
                throw new Exception("Credenciales inválidas");
            }

            await _localStorage.SetItemAsync(CURRENT_USER, userDto);
        }

        public async Task<bool> IsUserActive()
        {
            UsuarioDTO stored = await _localStorage.GetItemAsync<UsuarioDTO>(CURRENT_USER);
            return stored is not null;
        }

        public async Task LogOut()
        {
            await _localStorage.RemoveItemAsync(CURRENT_USER);
        }

        public async Task<UsuarioDTO?> GetCurrentUser()
        {
            UsuarioDTO? stored = await _localStorage.GetItemAsync<UsuarioDTO>(CURRENT_USER);
            if (stored == null)
            {
                return null;
            }

            UsuarioDTO usuario = _usuarioController.GetById(stored.Id);
            return usuario;
        }
    }
}