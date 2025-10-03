namespace EntryLog.Web.Models;

// Se crear con la finalidad de obtener l
// los claims del usuario autenticado de forma tipada
public record UserVM(
    int DocumentNumber,
    string Rol,
    string Email,
    string Name
);
