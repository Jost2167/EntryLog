using System.Security.Claims;
using EntryLog.Web.Models;

namespace EntryLog.Web.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static UserVM? GetUserData(this ClaimsPrincipal principal)
    {
        // Proceso de extracción de datos del usuario desde los claims
        
        // 1. Obtener la identidad del usuario con el fin de verificar si el usuario esta autenticado
        // de los contrario, no seria posible extraer los claims
        
        var identity = principal.Identity;
        
        if (identity != null && identity.IsAuthenticated)
        {
            // 2. Extraer los claims
            List<Claim> claims = principal.Claims.ToList();

            // 3. Mapear los claims a un objeto UserVM
            return new UserVM(
                DocumentNumber: int.Parse(claims.First(c=>c.Type == ClaimTypes.NameIdentifier).Value),
                Email: claims.First(c=>c.Type == ClaimTypes.Email).Value,
                Rol: claims.First(c=>c.Type == ClaimTypes.Role).Value,
                Name: claims.First(c=>c.Type == ClaimTypes.Name).Value
            );
        }
        else
        {
            return null;
        }
    }
}