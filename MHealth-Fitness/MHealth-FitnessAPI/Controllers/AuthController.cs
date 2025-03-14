using Azure.Core;
using MHealth_Fitness.Shared.DTO;
using MHealth_FitnessAPI.Data;
using MHealth_FitnessAPI.Models;
using MHealth_FitnessAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Security.Authentication;

namespace MHealth_FitnessAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        //constructor
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        //post Login 
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            try
            {
                // Call the login method from the service
                var user = await _authService.Login(login);
                // Return the user details
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // post login for Admin or Mod
        [HttpPost("LoginAuth")]
        public async Task<IActionResult> LoginAuth([FromBody] LoginDto login)
        {
            try
            {
                // Call the login method from the service
                var authUser = await _authService.LoginAuth(login);
                return Ok(authUser);
            }
            catch (ArgumentException ex)
            {
                // Return a 400 Bad Request for argument exceptions
                return BadRequest(ex.Message);
            }
            catch (AuthenticationException ex)
            {
                // Return a 403 Forbidden for authentication exceptions
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error for all other exceptions
                return BadRequest(ex.Message);
            }
        }

        // get user by username for forgot password
        [HttpGet("verify-user/{userName}")]
        public async Task<IActionResult> VerifyUser(string userName)
        {
            try
            {
                // Call the verify user method from the service
                var user = await _authService.VerifyUser(userName);
                // Return the user details
                return Ok(new { message = "User found.", user });
            }
            catch (ArgumentException ex)
            {
                // Return a 400 Bad Request for argument exceptions
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                // Return a 404 Not Found for key not found exceptions
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error for all other exceptions
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        // post reset password
        [HttpPost("reset-password/{userName}")]
        public async Task<IActionResult> ResetPassword(string userName, [FromBody] ForgotPassDto forgotPass)
        {
            try
            {
                // Call the reset password method from the service
                var updatedUser = await _authService.ForgotPass(userName, forgotPass);
                // Return the updated user details
                return Ok(new { message = "Password reset successfully.", user = updatedUser });
            }
            catch (ArgumentException ex)
            {
                // Return a 400 Bad Request for argument exceptions
                return BadRequest(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Return a 403 Forbidden for unauthorized access exceptions
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                // Return a 500 Internal Server Error for all other exceptions
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }
    }


}
