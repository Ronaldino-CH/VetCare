namespace Domain.Entities
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public bool EstadoUsuario { get; set; }
        public DateTime FechaCrea { get; set; }
        public DateTime? FechaEdita { get; set; }

        public Rol Rol { get; set; } = null!;
        public ICollection<ChatMensaje> ChatMensajes { get; set; } = new List<ChatMensaje>();
    }
}
