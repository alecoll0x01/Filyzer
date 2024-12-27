using Filyzer.Attributes;
using Filyzer.Domain.Entities;
using Filyzer.Domain.Enums;
using Filyzer.Domain.Interfaces;
using Filyzer.Domain.Models.Requests;
using Filyzer.Domain.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Filyzer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserRepository userRepository, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly ILogger<UsersController> _logger = logger;

        [HttpGet]
        [RequireRole(UserRole.Admin)]
        [ProducesResponseType(typeof(List<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var (users, totalCount) = await _userRepository.GetPaginatedAsync(page, pageSize);

                var response = users.Select(UserResponse.FromUser).ToList();

                Response.Headers.Append("X-Total-Count", totalCount.ToString());
                Response.Headers.Append("X-Total-Pages", ((int)Math.Ceiling(totalCount / (double)pageSize)).ToString());

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, new { message = "Error retrieving users" });
            }
        }

        [HttpGet("{id}")]
        [RequireRole(UserRole.Admin)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                return Ok(UserResponse.FromUser(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user {UserId}", id);
                return StatusCode(500, new { message = "Error retrieving user" });
            }
        }


        [HttpPost]
        [RequireRole(UserRole.Admin)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || !IsValidEmail(request.Email))
                {
                    return BadRequest(new { message = "Invalid email format" });
                }
                var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Email already registered" });
                }

                var user = new User(
                    email: request.Email,
                    role: request.Role,
                    dailyRequestLimit: request.DailyRequestLimit
                );

                await _userRepository.CreateAsync(user);

                _logger.LogInformation("User created successfully: {UserId}", user.Id);

                var response = UserResponse.FromUser(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, new { message = "Error creating user" });
            }
        }

        [HttpPut("{id}")]
        [RequireRole(UserRole.Admin)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                if (!string.IsNullOrEmpty(request.Email))
                {
                    if (!IsValidEmail(request.Email))
                    {
                        return BadRequest(new { message = "Invalid email format" });
                    }

                    var existingUser = await _userRepository.GetByEmailAsync(request.Email);
                    if (existingUser != null && existingUser.Id != id)
                    {
                        return BadRequest(new { message = "Email already in use" });
                    }

                    user.UpdateEmail(request.Email);
                }

                if (request.DailyRequestLimit.HasValue)
                {
                    user.UpdateDailyRequestLimit(request.DailyRequestLimit.Value);
                }

                await _userRepository.UpdateAsync(user);

                _logger.LogInformation("User updated successfully: {UserId}", id);

                return Ok(UserResponse.FromUser(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                return StatusCode(500, new { message = "Error updating user" });
            }
        }

        [HttpDelete("{id}")]
        [RequireRole(UserRole.Admin)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                await _userRepository.DeleteAsync(id);

                _logger.LogInformation("User deleted successfully: {UserId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user {UserId}", id);
                return StatusCode(500, new { message = "Error deleting user" });
            }
        }


        [HttpPost("{id}/regenerate-key")]
        [RequireRole(UserRole.Admin)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RegenerateApiKey(Guid id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                user.GetGenerateApiKey();
                await _userRepository.UpdateAsync(user);

                _logger.LogInformation("API key regenerated successfully: {UserId}", id);

                return Ok(UserResponse.FromUser(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error regenerating API key for user {UserId}", id);
                return StatusCode(500, new { message = "Error regenerating API key" });
            }
        }


        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

    }
}
