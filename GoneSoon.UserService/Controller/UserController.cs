using GoneSoon.UserService.Dto;
using GoneSoon.UserService.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoneSoon.UserService.Controller
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var user = await _userService.FindOrCreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user is not null ? Ok(user) : NotFound();
        }

        [HttpGet("login")]
        public IActionResult Login()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return Unauthorized();

            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims.ToDictionary(c => c.Type, c => c.Value);

            // Пример: извлекаем нужные поля
            var email = claims?[ClaimTypes.Email];
            var name = claims?[ClaimTypes.NameIdentifier];
            var displayName = claims?[ClaimTypes.Name];

            // Тут логика: проверить в БД, создать, если новый
            var user = await _userService.FindOrCreateAsync(new CreateUserRequest
            {
                ExternalId = name!,
                Provider = "Google",
                DisplayName = displayName ?? "",
                Email = email ?? ""
            });

            return Ok(user); // или Redirect в UI
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var users = await _userService.GetAllAsync();
        //    return Ok(users);
        //}
    }

}
