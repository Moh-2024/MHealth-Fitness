using MHealth_Fitness.Shared.DTO;
using MHealth_FitnessAPI.Models;
using MHealth_FitnessAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace MHealth_FitnessAPI.Controllers
{
    [Route("api/[controller]")]
    public class ModController : ControllerBase
    {
        // service to call database
        private readonly ModService _modService; 
        // service for picture storage in azure blob
        private readonly BlobService _blobService;

        // Constructor
        public ModController(BlobService blobService, ModService modService)
        {
            _blobService = blobService;
            _modService = modService;
        }

        // Get all users
        [HttpGet("Users/{userName}")]
        public async Task<IActionResult> GetUsers(string userName)
        {
            try
            {
                // call get all user method in mod service 
                var users = await _modService.GetAllUsers(userName);
                // reuturn list of users
                return Ok(users);
            }
            catch (Exception ex)
            {
                // return error message
                return StatusCode(500, ex.Message);
            }
        }

        // Exercises---------------------------------------------------------------------------------------

        // Get all exercises
        [HttpGet("exercises")]
        public async Task<IActionResult> GetExercises()
        {
            try
            {
                // call mod service to get the the list of exercises
                var exercises = await _modService.GetAllExercises();
                // return ok with list if exercises
                return Ok(exercises);
            }
            catch (Exception ex)
            {
                // return exeception
                return StatusCode(500, ex.Message);
            }
        }

        // get a specific exercise by exercise id
        [HttpGet("exercises/{exerciseID}")]
        public async Task<IActionResult> GetExerciseByID(int exerciseID)
        {
            try
            {
                // call mod service to get the exercise
                var exercise = await _modService.GetExerciseByID(exerciseID);
                //return ok when exercise is found
                return Ok(exercise);
            }
            catch (Exception ex)
            {
                // exception error message
                return StatusCode(500, ex.Message);
            }
        }

        // Add exercise
        [HttpPost("addExercise/{userName}")]
        public async Task<IActionResult> AddExercise(IFormFile file1, IFormFile file2, IFormFile file3, string userName, [FromForm] ExerciseDto exercise)
        {
            try
            {
                if (exercise == null)
                {
                    // checking if exercise is empty
                    return BadRequest("Invalid exercise object.");
                }

                // creating a variable filename to have a unique identifier and getting the file type and adding it with the guid
                if (file1 != null && file1.Length > 0)
                {
                    var fileName1 = Guid.NewGuid().ToString() + Path.GetExtension(file1.FileName);
                    using var stream1 = file1.OpenReadStream();
                    var imageUrl1 = await _blobService.UploadImageAsync(stream1, fileName1);
                    exercise.ImageUrl1 = imageUrl1;
                }

                if (file2 != null && file2.Length > 0)
                {
                    var fileName2 = Guid.NewGuid().ToString() + Path.GetExtension(file2.FileName);
                    using var stream2 = file2.OpenReadStream();
                    var imageUrl2 = await _blobService.UploadImageAsync(stream2, fileName2);
                    exercise.ImageUrl2 = imageUrl2;
                }

                if (file3 != null && file3.Length > 0)
                {
                    var fileName3 = Guid.NewGuid().ToString() + Path.GetExtension(file3.FileName);
                    using var stream3 = file3.OpenReadStream();
                    var imageUrl3 = await _blobService.UploadImageAsync(stream3, fileName3);
                    exercise.ImageUrl3 = imageUrl3;
                }

                // calling mod service to add the exercise
                await _modService.AddExercise(userName, exercise);
                // returning ok 
                return Ok(new { Message = "Exercise created!" });
            }
            catch (Exception ex)
            {
                // if exception error
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost("updateExercise/{userName}/{exerciseID}")]
        public async Task<IActionResult> UpdateExercise(string userName, int exerciseID, [FromForm] ExerciseDto exercise, IFormFile file1, IFormFile file2, IFormFile file3)
        {
            try
            {
                if (exercise == null)
                {
                    // checking if exercise is empty
                    return BadRequest("Invalid exercise object.");
                }

                // checking file1 is not empty
                if (file1 != null && file1.Length > 0)
                {
                    // creating a variable filename to have a unique identifier and getting the file type and adding it with the guid
                    var fileName1 = Guid.NewGuid().ToString() + Path.GetExtension(file1.FileName);

                    // handling picture uploads
                    using var stream1 = file1.OpenReadStream();
                    // calling blob service to upload the image to azure blob storage
                    var imageUrl1 = await _blobService.UploadImageAsync(stream1, fileName1);
                    // Set the image URL to the exercise object
                    exercise.ImageUrl1 = imageUrl1;
                }

                // checking file2 is not empty
                if (file2 != null && file2.Length > 0)
                {
                    // creating a variable filename to have a unique identifier and getting the file type and adding it with the guid
                    var fileName2 = Guid.NewGuid().ToString() + Path.GetExtension(file2.FileName);

                    // handling picture uploads
                    using var stream2 = file2.OpenReadStream();
                    // calling blob service to upload the image to azure blob storage
                    var imageUrl2 = await _blobService.UploadImageAsync(stream2, fileName2);
                    // Set the image URL to the exercise object
                    exercise.ImageUrl2 = imageUrl2;
                }

                // checking file3 is not empty
                if (file3 != null && file3.Length > 0)
                {
                    // creating a variable filename to have a unique identifier and getting the file type and adding it with the guid
                    var fileName3 = Guid.NewGuid().ToString() + Path.GetExtension(file3.FileName);

                    // handling picture uploads
                    using var stream3 = file3.OpenReadStream();
                    // calling blob service to upload the image to azure blob storage
                    var imageUrl3 = await _blobService.UploadImageAsync(stream3, fileName3);
                    // Set the image URL to the exercise object
                    exercise.ImageUrl3 = imageUrl3;
                }

                // calling mod service to update the exercise
                await _modService.UpdateExercise(userName, exerciseID, exercise);
                // returning ok
                return Ok(new { Message = "Exercise updated!" });
            }
            catch (Exception ex)
            {
                // if exception error
                return StatusCode(500, ex.Message);
            }
        }


        // Delete exercise
        [HttpDelete("deleteExercise/{userName}/{exerciseID}")]
        public async Task<IActionResult> DeleteExercise(string userName, int exerciseID)
        {
            try
            {
                // calling mod service to delete the exercise
                await _modService.DeleteExercise(userName, exerciseID);
                // returning ok
                return Ok(new { Message = "Exercise deleted." });
            }
            catch (Exception ex)
            {
                // if exception error
                return StatusCode(500, ex.Message);
            }
        }


        //Body Parts---------------------------------------------------------------------------------------

        // Get all body parts
        [HttpGet("bodyParts")]
        public async Task<IActionResult> GetBodyParts()
        {
            try
            {
                // call mod service to get the list of body parts
                var bodyParts = await _modService.GetAllBodyParts();
                // return ok with list of body parts
                return Ok(bodyParts);
            }
            catch (Exception ex)
            {
                // return exception error message
                return StatusCode(500, ex.Message);
            }
        }

        // Add body part
        [HttpPost("addBodyPart/{userName}")]
        public async Task<IActionResult> AddBodyPart(string userName, [FromBody] BodyPart bodyPart)
        {
            try
            {
                // checking if body part is empty
                if (bodyPart == null)
                {
                    return BadRequest("Invalid body part object.");
                }
                // calling mod service to add the body part
                await _modService.AddBodyPart(userName, bodyPart);
                // returning ok
                return Ok(new { Message = "Body part created!" });
            }
            catch (Exception ex)
            {
                // if exception error
                return StatusCode(500, ex.Message);
            }
        }

        // Update body part
        [HttpPost("updateBodyPart/{userName}/{bodyPartID}")]
        public async Task<IActionResult> UpdateBodyPart(string userName, int bodyPartID, [FromBody] BodyPart bodyPart)
        {
            try
            {
                // checking if body part is empty
                if (bodyPart == null)
                {
                    return BadRequest("Invalid body part object.");
                }

                // calling mod service to update the body part
                await _modService.UpdateBodyPart(userName, bodyPartID, bodyPart);
                // returning ok
                return Ok(new { Message = "Body part updated!" });
            }
            catch (Exception ex)
            {
                // if exception error
                return StatusCode(500, ex.Message);
            }
        }

        // Delete body part
        [HttpDelete("deleteBodyPart/{userName}/{bodyPartID}")]
        public async Task<IActionResult> DeleteBodyPart(string userName, int bodyPartID)
        {
            try
            {
                // calling mod service to delete the body part
                await _modService.DeleteBodyPart(userName,bodyPartID);
                // returning ok
                return Ok(new { Message = "Body part deleted." });
            }
            catch (Exception ex)
            {
                // if exception error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
