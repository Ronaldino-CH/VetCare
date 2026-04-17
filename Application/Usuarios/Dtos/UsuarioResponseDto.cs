namespace Application.Usuarios.Dtos
{
    public class UsuarioResponseDto
    {
        public int IdUsuario { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public bool EstadoUsuario { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaEdita { get; set; }
    }
}