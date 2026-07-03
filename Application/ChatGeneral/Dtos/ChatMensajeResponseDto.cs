namespace Application.ChatGeneral.Dtos
{
    public class ChatMensajeResponseDto
    {
        public int IdMensaje { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public string RolNombre { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaEnvio { get; set; }
    }
}
