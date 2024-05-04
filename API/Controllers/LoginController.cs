using Microsoft.AspNetCore.Mvc;
using API.Models;
using API.Services.Interfaces;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _loginService;

        public LoginController(ILogin loginService)
        {
            _loginService = loginService;
        }

        [HttpGet("login")]
        public IActionResult Login([FromQuery] Login login)
        {
            string result = _loginService.GetUser(login);
            if (result.StartsWith("Inicio de sesión exitoso"))
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] Login login)
        {
            string result = _loginService.CreateUser(login);
            if (result == "Creación de usuario exitosa")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
