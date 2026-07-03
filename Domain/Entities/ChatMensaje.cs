namespace Domain.Entities
{
    public class ChatMensaje
    {
        public int IdMensaje { get; set; }
        public int IdUsuario { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; }
        public bool Activo { get; set; }

        public Usuario Usuario { get; set; } = null!;
    }
}
