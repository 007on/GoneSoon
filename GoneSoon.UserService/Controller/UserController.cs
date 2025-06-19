using GoneSoon.InteractionProtocol.UserService.Data;
using GoneSoon.UserService.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GoneSoon.UserService.Controller
{
    /// <summary>
    /// Controller for managing user-related operations.
    /// </summary>
    [ApiController]
    [Route("api/users")]
    [Authorize]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Creates a new user or retrieves an existing one.
        /// </summary>
        /// <param name="request">The user creation request.</param>
        /// <returns>The created or retrieved user.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            var user = await _userService.FindOrCreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user if found, otherwise a 404 status.</returns>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(long id)
        {
            var user = await _userService.GetByIdAsync(id);
            return user is not null ? Ok(user) : NotFound();
        }

        /// <summary>
        /// Initiates the Google login process.
        /// </summary>
        /// <returns>A challenge to authenticate with Google.</returns>
        [HttpGet("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public IActionResult Login()
        {
            var redirectUrl = Url.Action("GoogleResponse", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Handles the response from Google authentication.
        /// </summary>
        /// <returns>The authenticated user or an unauthorized status.</returns>
        [HttpGet("google-response")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
                return Unauthorized();

            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims.ToDictionary(c => c.Type, c => c.Value);

            var email = claims?[ClaimTypes.Email];
            var name = claims?[ClaimTypes.NameIdentifier];
            var displayName = claims?[ClaimTypes.Name];

            var user = await _userService.FindOrCreateAsync(new CreateUserRequest
            {
                ExternalId = name!,
                Provider = "Google",
                DisplayName = displayName ?? "",
                Email = email ?? ""
            });

            return Ok(user);
        }
    }
}
