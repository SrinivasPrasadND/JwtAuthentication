using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtAuth.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly JwtTokenManager _jwtTokenManager;

        private static readonly string[] Summaries = {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, JwtTokenManager jwtTokenManager)
        {
            _logger = logger;
            _jwtTokenManager = jwtTokenManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> GetWeatherForecast() =>
        Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        }).ToArray();


        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] UserRequest user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                _logger.LogError("invalid user request");
                return BadRequest(user);
            }
            var token = _jwtTokenManager.Authenticate(user);
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError("Unauthorized user access");
                return Unauthorized();
            }
            return Ok(new JsonResult(new { token }));
        }

        [Authorize(Roles = "HRManager,Finance,User")]
        [HttpGet(Name = "GetRoleContext")]
        public IActionResult Payslip() =>
            Content("User || HRManager || Finance");
    }
}