using MHealth_Fitness.Shared.DTO;
using MHealth_FitnessAPI.Models;
using MHealth_FitnessAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Authentication;

namespace MHealth_FitnessAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly AdminService _adminService;

        //constructor
        public AdminController(AdminService adminService)
        {
            _adminService = adminService;
        }

        // Get all roles
        [HttpGet("Roles/{userName}")]
        public async Task<IActionResult> GetUsers(string userName)
        {
            try
            {
                var roles = await _adminService.GetAllRoles(userName);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // update role of users
        [HttpPost("Update-Role/{userName}/{userID}")]
        public async Task<IActionResult> UpdateRole(string userName, int userID, [FromBody] UserDto user)
        {
            try
            {
                //calling the admin service with username to verify, userid and roleID
                await _adminService.UpdateRole(userName, userID, user.RoleID);
                //return ok if its successfull
                return Ok(new { Message = "Role Updated!" });

            }
            catch (AuthenticationException ex)
            {
                // exception if user is not admin or mod
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                // if the request is bad
                return BadRequest(ex.Message);
            }
        }

        // delete user
        [HttpDelete("Delete-User/{userName}/{userID}")]
        public async Task<IActionResult> DeleteRole(string userName, int userID)
        {
            try
            {
                //calling the admin service with username to verify, userid 
                await _adminService.DeleteUser(userName, userID);
                //return ok if its successfull
                return Ok(new { Message = "User deleted" });

            }
            // exception if user is not admin or mod
            catch (AuthenticationException ex)
            {
                
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
