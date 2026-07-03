using Application.ChatGeneral.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VetCare.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "1,2,3")]
    public class ChatGeneralController : ControllerBase
    {
        private readonly IChatGeneralService _chatGeneralService;

        public ChatGeneralController(IChatGeneralService chatGeneralService)
        {
            _chatGeneralService = chatGeneralService;
        }

        [HttpGet("historial")]
        public async Task<IActionResult> GetHistorial([FromQuery] int take = 50)
        {
            var result = await _chatGeneralService.GetHistorialRecienteAsync(take);
            return Ok(result);
        }
    }
}
