using System.Security.Claims;
using EntryLog.Business.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EntryLog.Web.Extensions;

public static class HttpContextExtensions
{
    public static async Task SignInCookieAsync(this HttpContext context, LoginResponseDTO data)
    {
        // Proceso de autenticacion usando cookies

        // 1. Crear los claims -> los claims son pares clave-valor que representan información sobre el usuario
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, data.DocumentNumber.ToString()),
            new Claim(ClaimTypes.Email, data.Email),
            new Claim(ClaimTypes.Role, data.Rol)
        };
        
        // 2. Crear una identidad -> la identidad es un conjunto de claims, ademas del tipo de autenticacion
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        
        // 3. Crear un principal -> el principal es un conjunto de identidades que representa al usuario
        var principal = new ClaimsPrincipal(identity);
        
        // 4. Crear las propiedades de autenticacion -> las propiedades son metadatos sobre la autenticacion
        var authProperties = new AuthenticationProperties
        {
            AllowRefresh = true, // Permite refrescar la cookie, es decir, renovar su tiempo de expiracion si el usuario sigue activo (movimeintos)
            IsPersistent = true, // La cookie persiste aunque el navegador se cierre
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // Tiempo de expiracion de la cookie (30 minutos)
        };
        
        // 5. Autenticar al usuario -> se crea la cookie de autenticacion y se envia al cliente
        await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);
        
        // Nota:
        // - El servidor no guarda la cookie, es el cliente quien la guarda y la envia en cada peticion
        // - La cookie contiene un token que representa al usuario, no contiene la informacion del usuario
        // - El servidor valida el token en cada peticion para autenticar al usuario
    }

    public static async Task SignOutCookieAsync(this HttpContext context)
    {
        // Limpiar la sesion
        context.Session.Clear();
        
        // Cerrar la sesion del usuario -> se elimina la cookie de autenticacion del cliente
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}