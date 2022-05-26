using LimiterApplication.Services.Interfaces;
using LimiterApplication.Services.Models.Input;
using LimiterApplication.Services.Models.Output;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LimiterApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;
        private readonly HttpClient _httpClient;
        public UsersController(IUserService userService, ILogger<UsersController> logger, IConfiguration configuration)
        {
            _userService = userService;
            _logger = logger;
            _httpClient = new HttpClient { BaseAddress = new Uri(configuration.GetSection("WebSiteUrl").Value) };
        }

        [HttpGet]
        public async Task<IEnumerable<UserOutputModel>?> Get()
        {
            try
            {
                var users = _userService.GetUsers();
                var result = await _httpClient.GetAsync(_httpClient.BaseAddress);
                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [HttpGet("{id}")]
        public async Task<UserOutputModel?> Get(Guid id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                var result = await _httpClient.GetAsync(_httpClient.BaseAddress);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [HttpPost]
        public async Task<UserOutputModel?> Post([FromBody] UserInputModel model)
        {
            try
            {
                var user = await _userService.CreateUser(model);
                var json = JsonSerializer.Serialize(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await _httpClient.PostAsync(_httpClient.BaseAddress, data);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [HttpPut("{id}")]
        public async Task<UserOutputModel?> Put(Guid id, [FromBody] UserInputModel model)
        {
            try
            {
                var user = await _userService.UpdateUser(id, model);
                var json = JsonSerializer.Serialize(model);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await _httpClient.PutAsync(_httpClient.BaseAddress, data);
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return null;
            }
        }

        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            try
            {
                var result = await _httpClient.DeleteAsync(id.ToString());
                await _userService.DeleteUser(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
