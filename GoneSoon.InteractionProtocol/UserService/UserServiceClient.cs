using GoneSoon.InteractionProtocol.UserService.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GoneSoon.InteractionProtocol.UserService
{
    public class UserServiceClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;

        public UserServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Creates a new user or retrieves an existing one.
        /// </summary>
        /// <param name="request">The user creation request.</param>
        /// <returns>The created or retrieved user.</returns>
        public async Task<UserDto> CreateUserAsync(CreateUserRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/users", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        /// <summary>
        /// Retrieves a user by their ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user if found, otherwise null.</returns>
        public async Task<UserDto> GetUserByIdAsync(long id)
        {
            var response = await _httpClient.GetAsync($"api/users/{id}");
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        /// <summary>
        /// Initiates the Google login process.
        /// </summary>
        /// <returns>The URL for Google login.</returns>
        public string GetGoogleLoginUrl()
        {
            return $"{_httpClient.BaseAddress}api/users/login";
        }

        /// <summary>
        /// Handles the response from Google authentication.
        /// </summary>
        /// <returns>The authenticated user.</returns>
        public async Task<UserDto> HandleGoogleResponseAsync()
        {
            var response = await _httpClient.GetAsync("api/users/google-response");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return null;
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }
    }
}
