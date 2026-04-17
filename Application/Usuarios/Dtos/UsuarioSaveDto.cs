namespace Application.Usuarios.Dtos
{
    public class UsuarioSaveDto
    {
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public int IdRol { get; set; }
    }
}