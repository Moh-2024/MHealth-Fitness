using MHealth_Fitness.Shared.DTO;
using MHealth_FitnessAPI.Data;
using MHealth_FitnessAPI.Models;
using MHealth_FitnessAPI.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Security.Claims;

namespace MHealth_FitnessAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;

        // Constructor
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        //User----------------------------------------------------------------------------------------------

        // Register a user
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto user)
        {
            try
            {
                // setting user role to 3 as defualt
                user.RoleID = 3;
                // calling user service to register user
                await _userService.Register(user);
                // returning success message
                return Ok(new { Message = "User created!" });
            }
            catch (Exception ex)
            {
                // returning error message
                return BadRequest(ex.Message);
            }

        }

        // Update a user
        [HttpPost("Update-User/{userID}")]
        public async Task<IActionResult> UpdateUser(int userID, [FromBody] UserDto user)
        {
            try
            {
                // calling user service to update user
                await _userService.UpdatUser(userID, user);
                // returning success message
                return Ok(new { Message = "Updated user." });
            }
            catch (Exception ex)
            {
                // returning error message
                return BadRequest(ex.Message);
            }

        }

        // Get a user by ID
        [HttpGet("User/{userID}")]
        public async Task<IActionResult> GetUserByID(int userID)
        {
            try
            {
                // calling user service to get user by ID
                var user = await _userService.GetByID(userID);
                // returning user
                return Ok(user);
            }
            catch (Exception ex)
            {
                // returning error message
                return StatusCode(500, ex.Message);
            }
        }


        //routines-----------------------------------------------------------------------------------------------

        // Get all rotuines
        [HttpGet("Routines/{userID}")]
        public async Task<IActionResult> GetRoutines(int userID)
        {
            try
            {
                // calling user service to get all routines
                var routines = await _userService.GetAllRoutine(userID);
                // returning routines
                return Ok(routines);
            }
            catch (Exception ex)
            {
                // returning error message
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("Routine/{routineID}")]
        public async Task<IActionResult> GetRoutineByID(int routineID)
        {
            try
            {
                // calling user service to get routine by ID
                var routine = await _userService.GetRoutineByID(routineID);
                // returning user
                return Ok(routine);
            }
            catch (Exception ex)
            {
                // returning error message
                return StatusCode(500, ex.Message);
            }
        }


        // Add a routine
        [HttpPost("Add-Routine/{userID}")]
        public async Task<IActionResult> AddRoutine([FromBody] RoutineDto routine)
        {
            try
            {
                // calling user service to add routine
                await _userService.AddRoutine(routine);
                // returning success message
                return Ok(new { Message = "Routine created!" });
            }
            catch (Exception ex)
            {
                // returning error message
                return BadRequest(ex.Message);
            }

        }

        // Update a routine
        [HttpPost("Update-Routine/{routineID}")]
        public async Task<IActionResult> UpdateRoutine(int routineID, [FromBody] RoutineDto routine)
        {
            try
            {
                // calling user service to update routine
                await _userService.UpdateRoutine(routineID, routine);
                // returning success message
                return Ok(new { Message = "Updated routine." });
            }
            catch (Exception ex)
            {
                // returning error message
                return BadRequest(ex.Message);
            }

        }

        // Delete a routine
        [HttpDelete("Delete-Routine/{routineID}")]
        public async Task<IActionResult> DeleteRoutine(int routineID)
        {
            try
            {
                // calling user service to delete routine
                await _userService.DeleteRoutine(routineID);
                // returning success message
                return Ok(new { Message = "Deleted routine" });
            }
            catch (Exception ex)
            {
                // returning error message
                return BadRequest(ex.Message);
            }

        }

        //routine exercises------------------------------------------------------------------------------------------------

        // get a list of routine exercises with routine id.
        [HttpGet("RoutineExercises/{routineID}")]
        public async Task<IActionResult> GetRoutineExercises(int routineID)
        {
            try
            {
                // calling user service to get all routine exercises
                var routineExercises = await _userService.GetAllRoutineExercise(routineID);
                // returning routine exercises
                return Ok(routineExercises);
            }
            catch (Exception ex)
            {
                // returning error message
                return StatusCode(500, ex.Message);
            }
        }

        // add exercise to routine
        [HttpPost("Add-RoutineExercise")]
        public async Task<IActionResult> AddRoutineExercise([FromBody] RoutineExercise routineExercise)
        {
            try
            {
               // calling user service to add routine exercise
                await _userService.AddRoutineExercise(routineExercise);
                // returning success message
                return Ok(new { Message = "Routine Exericse Added!" });
            }
            catch (Exception ex)
            {
                // returning error message
                return BadRequest(ex.Message);
            }

        }

        // update routine exercise
        [HttpPost("Update-RoutineExercise/{routineExerciseID}")]
        public async Task<IActionResult> UpdateRoutineExercise(int routineExerciseID, [FromBody] RoutineExercise routineExercise)
        {
            try
            {
                // calling user service to update routine exercise
                await _userService.UpdateRoutineExercise(routineExerciseID, routineExercise);
                // returning success message.
                return Ok(new { Message = "Updated routine Exercise." });
            }
            catch (Exception ex)
            {
                // returning error message
                return BadRequest(ex.Message);
            }

        }

        // delete routine exercise
        [HttpDelete("Delete-RoutineExercise/{routineExerciseID}")]
        public async Task<IActionResult> DeleteRoutineExercise(int routineExerciseID)
        {
            try
            {
                // calling user service to delete routine exercise
                await _userService.DeleteRoutineExercise(routineExerciseID);
                // returning successfull message
                return Ok(new { Message = "Deleted routine exercise" });
            }
            catch (Exception ex)
            {
                // returning error message
                return BadRequest(ex.Message);
            }

        }

        //SetsPerExercise------------------------------------------------------------------------------------------------

        // Get all sets for a given routine exercise ID
        [HttpGet("SetsPerExercise/{routineExerciseID}")]
        public async Task<IActionResult> GetSetsPerExercise(int routineExerciseID)
        {
            try
            {
                var sets = await _userService.GetAllSetsPerExercise(routineExerciseID);
                return Ok(sets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Add a set to a routine exercise
        [HttpPost("Add-SetPerExercise")]
        public async Task<IActionResult> AddSetPerExercise([FromBody] SetsPerExerciseDto set)
        {
            try
            {
                await _userService.AddSetPerExercise(set);
                return Ok(new { Message = "Set Added!" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Update a set by ID
        [HttpPost("Update-SetPerExercise/{setID}")]
        public async Task<IActionResult> UpdateSetPerExercise(int setID, [FromBody] SetsPerExerciseDto set)
        {
            try
            {
                await _userService.UpdateSetPerExercise(setID, set);
                return Ok(new { Message = "Updated Set." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete a set by ID
        [HttpDelete("Delete-SetPerExercise/{setID}")]
        public async Task<IActionResult> DeleteSetPerExercise(int setID)
        {
            try
            {
                await _userService.DeleteSetPerExercise(setID);
                return Ok(new { Message = "Deleted Set." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //Favorites--------------------------------------------------------

        // Get all favorites
        [HttpGet("Favorites/{userID}")]
        public async Task<IActionResult> GetFavorites(int userID)
        {
            try
            {
                // calling user service to get all favorites
                var favorites = await _userService.GetByFavoriteID(userID);
                // returning favorites
                return Ok(favorites);
            }
            // returning error message
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Add a favorite
        [HttpPost("Add-Favorite")]
        public async Task<IActionResult> AddFavorites([FromBody] Favorite favorite)
        {
            try
            {
                // Check if the exercise is already favorited by the user
                var existingFavorite = await _userService.GetFavoriteByUserAndExercise(favorite.UserID, favorite.ExerciseID);
                // If the exercise is already favorited, return a bad request
                if (existingFavorite != null)
                {
                    return BadRequest("This exercise is already in your favorites.");
                }

                // Add the favorite
                await _userService.AddFavorite(favorite);
                // Return a success message
                return Ok(new { Message = "Favorite Added" });
            }
            // return an error message
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete a favorite
        [HttpDelete("Delete-Favorite/{userID}/{favoriteID}")]
        public async  Task<IActionResult> DeleteFavorites(int userID, int favoriteID)
        {
            try
            {
                // Delete the favorite
                await _userService.DeleteFavorite(userID, favoriteID);
                // Return a success message
                return Ok(new { Message = "Deleted Favorite" });
            }
            // Return an error message
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
